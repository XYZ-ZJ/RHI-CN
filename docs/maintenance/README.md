# RHI 简体中文项目维护说明

## 维护原则与开发模式

- **远程编译与自动发布**：所有可执行文件（`RHI.exe`、`RHI.DropHelper.exe`、ZIP 便携包及安装程序）全部由远程仓库的 GitHub Actions 工作流在云端 Windows 环境中自动编译与发布。
- **本地工作区职责**：本地仅进行界面翻译、汉化调优、静态代码审查与语法合规性检查，不生成或留存任何本地构建缓存与编译产物。

---

## 当前状态

- 状态核对日期：2026-10-21
- 工作区路径：`C:\Users\56892\Documents\Codex\RHI汉化`
- 汉化远程仓库：[XYZ-ZJ/RHI-CN](https://github.com/XYZ-ZJ/RHI-CN)
- 上游源仓库：[RankFTW/RHI](https://github.com/RankFTW/RHI)
- 分支：`main`；基础提交：`b0570336431bfce265e0f6dc19156ea751e404f7`
- 程序版本：`2.8.7.0`
- 汉化方案：源码内置简体中文，无外部翻译 API 依赖。

汉化覆盖主窗口、侧边栏、设置面板、游戏详情、全量悬停提示（Tooltip）、对话框、安装进度、NVIDIA 驱动设置以及内置快速入门指南（FAQ）。当前静态审计队列中所有 205 处 Tooltip 及界面文本英文短语候选已全量复核并汉化完成（待审英文短语候选为 0）。

覆盖仓库全部 320 个 C# 文件的 Roslyn 语法检查无语法错误（`syntax_errors: []`）。

---

## 本地维护与代码审查命令

在本地仓库根目录下运行只读审查：

```powershell
# 1. 扫描界面文案与 Tooltip 英文候选
python tools\maintenance\audit_project.py --output-dir docs\maintenance

# 2. 执行 Roslyn 静态语法与代码结构审查
& tools\maintenance\audit-source.ps1 -OutputFile docs\maintenance\source-audit.json

# 3. 检查代码格式与空白字符规范
git diff --check
```

审查通过后直接推送到远程仓库：
```powershell
git push rhi-cn main
```
推送后，GitHub Actions 云端工作流将自动触发远程编译并在 Releases 中发布最新版本。
