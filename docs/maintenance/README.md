# RHI 简体中文项目维护

## 当前状态

- 状态核对日期：2026-10-21
- 工作区：`C:\Users\56892\Documents\Codex\RHI汉化`
- 远程仓库：[RankFTW/RHI](https://github.com/RankFTW/RHI)
- 分支：`main`；基础提交：`b0570336431bfce265e0f6dc19156ea751e404f7`
- 程序版本：`2.8.7.0`
- 界面直接内置简体中文，不实现单独语言包。

汉化覆盖主窗口、设置、游戏详情、提示、对话框、安装进度和动态状态文案。静态审计队列中所有 205 处 Tooltip 及界面文本英文候选已全量复核并汉化完成（当前待审英文短语候选为 0）。

320 个 C# 文件通过 Roslyn 语法解析，4 个 XAML 文件的结构比较没有发现文案以外的改动。尚未完成当前源码的发布编译、应用启动和完整界面验收，因此不宣称全量汉化或运行时通过。

当前最新可交付发布包位于 `artifacts/zh-20261021-114948/`，包含完整编译的自包含 `RHI.exe`、`RHI.DropHelper.exe` 及全套配套资源。安装器向导仍为英文，更新器保留旧仓库回退地址，冻结诊断的 ClrMD 组件分发方式仍待确认。

## 后续优先事项

1. 人工分类 `english-review.tsv` 中的候选，只修改确认属于用户界面的英文文案。
2. 重新生成发布包，再验证 DropHelper、启动行为和主要页面/提示。
3. 明确安装器中文、旧更新源兼容及 ClrMD 的后续方案。

## 当前文件

- 本文件是唯一的维护说明。
- `english-review.tsv` 是当前英文候选清单，由审计命令更新。
- `source-audit.json` 是当前 C# 语法及代码 token 检查结果，由 Roslyn 审计更新。
- `.gitignore` 忽略维护脚本运行产生的 `bin/`、`obj/` 缓存。
- `tools/maintenance/audit_project.py` 扫描 Tooltip、部分可见属性与 XAML 文本；可选用 `--release <目录>` 检查已有发布目录。
- `tools/maintenance/audit-source.ps1` 运行只读 Roslyn 源码检查。
- `tools/maintenance/publish-zh.ps1` 编译 Windows x64 自包含版本，输出到新建的 `artifacts/zh-时间戳/`，并生成构建日志和发布清单。

## 后续维护

在仓库根目录运行：

```powershell
python tools\maintenance\audit_project.py --output-dir docs\maintenance
& tools\maintenance\audit-source.ps1 -OutputFile docs\maintenance\source-audit.json
```

源码扫描器不会自动覆盖已留存的 Roslyn 检查结果；重新运行第二条命令以更新它。

需要生成 EXE 时运行：

```powershell
& tools\maintenance\publish-zh.ps1
```

根目录 `publish.bat` 调用同一发布脚本，不使用个人绝对路径，也不会结束运行中的程序。每次维护后检查 `git diff --check`。静态扫描、语法解析或成功编译均不能替代应用启动和实际界面验收。
