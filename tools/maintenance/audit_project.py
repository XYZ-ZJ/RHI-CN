"""Read-only RHI localization/package audit. Requires Python 3, no third-party packages.

The English inventory is a review queue, not a completion percentage. Indirect
tooltip sources, remote content, and runtime branches still require review.
"""
from __future__ import annotations

import argparse
import csv
from datetime import date
import hashlib
import json
import re
import subprocess
from pathlib import Path
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
HAN = re.compile(r"[\u3400-\u9fff]")
ENGLISH = re.compile(r"\b[A-Za-z]{3,}\s+[a-z]{2,}\b")
# Literal extraction is deliberately conservative; it is not a C# parser.
LITERAL = re.compile(r'\$?@?"(?:\\.|""|[^"\\])*"')
VISIBLE = re.compile(r'\b(?:Text|Content|Header|PlaceholderText|Title|PrimaryButtonText|SecondaryButtonText|CloseButtonText|OnContent|OffContent)\s*=')
XAML_VISIBLE = {
    "Text", "Content", "Header", "PlaceholderText", "Title", "PrimaryButtonText",
    "SecondaryButtonText", "CloseButtonText", "OnContent", "OffContent",
    "ToolTipService.ToolTip", "AutomationProperties.Name", "AutomationProperties.HelpText",
}


def english_value(value: str) -> bool:
    return bool(ENGLISH.search(value) and not HAN.search(value)
                and "http://" not in value and "https://" not in value)


def call_end(source: str, start: int) -> int:
    """Find a tooltip call's closing parenthesis, skipping strings and comments."""
    pos = source.find("(", start)
    depth = 0
    while pos < len(source):
        if source.startswith("//", pos):
            pos = source.find("\n", pos)
            if pos < 0:
                return len(source)
            continue
        if source.startswith("/*", pos):
            finish = source.find("*/", pos + 2)
            pos = finish + 2 if finish >= 0 else len(source)
            continue
        if source[pos] == '"':
            verbatim = pos > 0 and source[pos - 1] == "@"
            pos += 1
            while pos < len(source):
                if source[pos] == '"':
                    if verbatim and source.startswith('""', pos):
                        pos += 2
                        continue
                    pos += 1
                    break
                if not verbatim and source[pos] == "\\":
                    pos += 2
                else:
                    pos += 1
            continue
        if source[pos] == "(":
            depth += 1
        elif source[pos] == ")":
            depth -= 1
            if depth == 0:
                return pos + 1
        pos += 1
    return len(source)


def english_literals(text: str) -> list[str]:
    return [match.group() for match in LITERAL.finditer(text)
            if ENGLISH.search(match.group()) and not HAN.search(match.group())
            and "http://" not in match.group() and "https://" not in match.group()]


def source_files():
    for path in sorted((ROOT / "RenoDXCommander").rglob("*")):
        if path.suffix not in (".cs", ".xaml"):
            continue
        if any(part in ("bin", "obj", "original") for part in path.parts):
            continue
        yield path


def inspect_sources():
    queue = []
    tooltip_calls = 0
    english_calls = 0
    for path in source_files():
        source = path.read_text(encoding="utf-8-sig")
        rel = path.relative_to(ROOT).as_posix()
        if path.suffix == ".cs":
            for match in re.finditer(r"\bToolTipService\.SetToolTip\s*\(", source):
                line_start = source.rfind("\n", 0, match.start()) + 1
                if source[line_start:match.start()].lstrip().startswith("//"):
                    continue
                tooltip_calls += 1
                strings = english_literals(source[match.start():call_end(source, match.start())])
                if strings:
                    english_calls += 1
                    queue.append({"file": rel, "line": source.count("\n", 0, match.start()) + 1,
                                  "category": "tooltip_literal", "text": " | ".join(strings),
                                  "review": "pending"})
            for line_no, line in enumerate(source.splitlines(), 1):
                if line.lstrip().startswith("//") or "SetToolTip" in line:
                    continue
                if VISIBLE.search(line):
                    strings = english_literals(line)
                    if strings:
                        queue.append({"file": rel, "line": line_no,
                                      "category": "visible_assignment_candidate", "text": " | ".join(strings),
                                      "review": "pending"})
        else:
            root = ET.fromstring(source)  # Reject malformed XAML/XML.
            for element in root.iter():
                for key, value in element.attrib.items():
                    local = key.rsplit("}", 1)[-1]
                    if local in XAML_VISIBLE and english_value(value):
                        queue.append({"file": rel, "line": "", "category": "xaml_attribute_candidate",
                                      "text": f"{local}={value}", "review": "pending"})
                if element.text and english_value(element.text.strip()):
                    queue.append({"file": rel, "line": "", "category": "xaml_text_candidate",
                                  "text": element.text.strip(), "review": "pending"})
    return queue, tooltip_calls, english_calls


def git_info(*args):
    return subprocess.run(["git", *args], cwd=ROOT, capture_output=True,
                          text=True, encoding="utf-8", errors="replace", check=True).stdout.strip()


def xaml_structure():
    visible = {"Text", "Content", "Header", "PlaceholderText", "ToolTipService.ToolTip",
               "OnContent", "OffContent", "Title"}
    changes = []
    checked = 0
    for path in source_files():
        if path.suffix != ".xaml":
            continue
        rel = path.relative_to(ROOT).as_posix()
        original = subprocess.run(["git", "show", "HEAD:" + rel], cwd=ROOT, capture_output=True)
        if original.returncode:
            continue
        old = list(ET.fromstring(original.stdout).iter())
        new = list(ET.parse(path).getroot().iter())
        checked += 1
        if len(old) != len(new):
            changes.append({"file": rel, "reason": "element_count", "old": len(old), "new": len(new)})
        for index, (left, right) in enumerate(zip(old, new)):
            if left.tag != right.tag:
                changes.append({"file": rel, "reason": "element_tag", "index": index})
                continue
            for key in set(left.attrib) | set(right.attrib):
                before, after = left.get(key), right.get(key)
                if before == after:
                    continue
                local = key.rsplit("}", 1)[-1]
                if local in visible and not (str(before).startswith("{") or str(after).startswith("{")):
                    continue
                changes.append({"file": rel, "index": index, "attribute": key, "old": before, "new": after})
    return {"checked_files": checked, "non_text_attribute_or_structure_changes": changes}


def audit(release: Path | None):
    project = ET.parse(ROOT / "RenoDXCommander/RenoDXCommander.csproj").getroot()
    assets = [item.attrib["Include"].replace("\\", "/")
              for group in project.findall("ItemGroup") for item in group
              if item.findtext("ExcludeFromSingleFile") == "true"
              and "$(" not in item.attrib.get("Include", "")]
    assets += ["reshade.rdr2.ini", "RHI.DropHelper.exe", "LICENSE"]
    exe = release / "RHI.exe" if release else None
    data = exe.read_bytes() if exe and exe.exists() else b""
    labels = {
        "translated_browse": "浏览用于启动游戏的可执行文件。",
        "translated_mfg": "配置 NVIDIA 多帧生成：模式、帧数倍数和动态目标帧率。需要 50 系列显卡。",
        "translated_report": "将此游戏的诊断报告复制到剪贴板，便于在 Discord 或 GitHub 中寻求支持。",
        "english_driver_tooltip": "Vertical Sync settings — controls how the driver synchronizes frame rendering with your display's refresh rate.",
        "english_renodx_fallback": "RenoDX is an HDR mod framework that upgrades SDR games to HDR using ReShade. It provides per-game tone mapping and color space conversion.",
    }
    queue, calls, english_calls = inspect_sources()
    status = git_info("status", "--porcelain=v1", "-uno").splitlines()
    actual_diff = git_info("diff", "--name-only").splitlines()
    facts = {
        "audit_date": date.today().isoformat(), "root": str(ROOT), "release_dir": str(release) if release else None,
        "origin": git_info("remote", "get-url", "origin"), "base_commit": git_info("rev-parse", "HEAD"),
        "tracked_status_entries": len(status), "tracked_files_with_content_diff": len(actual_diff),
        "main_project_assembly_version": project.findtext("PropertyGroup/AssemblyVersion"),
        "exe_size": len(data), "exe_sha256": hashlib.sha256(data).hexdigest().upper() if data else None,
        "binary_text_checks": ({key: (value.encode("utf-8") in data or value.encode("utf-16le") in data)
                                for key, value in labels.items()} if exe else None),
        "declared_loose_assets": len(assets) - 3,
        "expected_files": assets,
        "missing_release_files": ([name for name in assets if not (release / name).is_file()]
                                  if release else None),
        "tooltip_assignment_calls": calls, "calls_with_english_sentence_literals": english_calls,
        "english_review_queue_is_not_completion_rate": True,
        "review_queue_rows": len(queue),
        "xaml_structure_comparison": xaml_structure(),
        "limits": ["English inventory uses lexical heuristics on C# tooltip/visible assignments and selected XAML attributes/text; indirect bindings, external content, comments, and runtime-only strings require separate review.",
                   "No complete translation percentage can be inferred from this queue.",
                   "Binary string presence does not prove UI behavior or package completeness.",
                   "No application launch, hardware writes, build, tests, or remote freshness check performed."],
    }
    return facts, queue


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--release", type=Path, help="Optional published folder to audit alongside source strings.")
    parser.add_argument("--output-dir", type=Path, help="Write JSON facts and a UTF-8 TSV review queue")
    args = parser.parse_args()
    facts, queue = audit(args.release.resolve() if args.release else None)
    if args.output_dir:
        args.output_dir.mkdir(parents=True, exist_ok=True)
        with (args.output_dir / "english-review.tsv").open("w", encoding="utf-8", newline="") as stream:
            writer = csv.DictWriter(stream, fieldnames=["file", "line", "category", "text", "review"], delimiter="\t")
            writer.writeheader()
            writer.writerows(queue)
    summary_keys = (
        "tooltip_assignment_calls", "calls_with_english_sentence_literals", "review_queue_rows"
    )
    if args.release:
        summary_keys += ("exe_sha256", "declared_loose_assets", "missing_release_files")
    print(json.dumps({key: facts[key] for key in summary_keys}, ensure_ascii=True, indent=2))


if __name__ == "__main__":
    main()
