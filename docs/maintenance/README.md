# RHI 简体中文项目维护文档

本文档为 **RHI (ReShade HDR 安装器) 简体中文版** 的核心维护指南，详细记录了项目架构定位、开发维护原则、源码模块索引、日常审查工作流、安装包配置及持续集成部署规范。

---

## 1. 项目概况与架构定位

- **中文项目仓库**：[XYZ-ZJ/RHI-CN](https://github.com/XYZ-ZJ/RHI-CN)
- **上游原项目**：[RankFTW/RHI](https://github.com/RankFTW/RHI)（原作者：RankFTW）
- **技术技术栈**：
  - **核心框架**：C# 12 / .NET 8 (目标平台 `net8.0-windows10.0.19041.0`，自包含 win-x64)
  - **UI 界面层**：WinUI 3 / Windows App SDK 2.5.1
  - **辅助工具**：`RHI.DropHelper`（基于 Windows Forms 实现的非提权拖放代理辅助程序）
  - **安装向导**：Inno Setup 6 脚本打包（集成官方简体中文语言包）
  - **持续集成**：GitHub Actions（Windows 云端自动编译、单文件打包与 GitHub Releases 自动发布）
- **汉化方案**：
  - 直接在源码中内置简体中文（不维护独立的动态资源包或外部 API 翻译层）。
  - 所有用户可见界面、悬停提示（Tooltip）、右键菜单、操作按钮、设置选项及内置“快速入门指南 (FAQ)”均已全量汉化。

---

## 2. 核心维护原则与开发模式

在进行任何代码修改、本地化更新或功能维护时，必须严格遵守以下原则：

### 2.1 远程编译与自动发布（核心规范）
- **本地环境**：仅用于代码汉化、文本调优、静态代码审查与语法合规性检查。**本地不执行编译发布，不生成也不提交任何 `bin/`、`obj/` 或 `artifacts/` 等本地构建缓存**。
- **云端编译**：所有可执行文件（`RHI-vX.X.X-zh-CN.exe` 等）全部由 GitHub Actions 云端虚拟环境自动拉取远程仓库代码进行干净构建、打包与发布。

### 2.2 技术标识与兼容性保护（红线原则）
在汉化过程中，**严禁修改**以下具有底层技术逻辑关联的文本：
1. **专有名词与品牌**：`ReShade`、`RenoDX`、`DLSS`、`Streamline`、`OptiScaler`、`Reflex`、`Vulkan`、`DXVK`、`Luma`、`DG2`、`MFG`、`RE Framework` 等。
2. **配置文件与路径标识**：`reshade.ini`、`Engine.ini`、`relimiter.ini`、`DisplayCommander.ini`、`dxvk.conf`、`icon.ico`、`7z.exe`、`7z.dll` 等。
3. **驱动与引擎键值**：`r.LUT.UpdateEveryFrame=1`、`HUDFix=true`、`DisableFlipMetering=true`、`nxm://` 协议名等。
4. **字符串插值变量**：诸如 `{currentDllName}`、`{gameName}`、`{ex.Message}`、`{count}` 等 C# 表达式占位符必须完整保留。

---

## 3. 源码汉化模块速查索引

各功能模块与界面文本在源码中的核心分布如下，便于快速定位修改：

| 界面区域 / 功能模块 | 对应源码文件 | 包含内容说明 |
|---|---|---|
| **主窗口 XAML 布局** | `RenoDXCommander/MainWindow.xaml` | 顶栏、侧边栏标题、全局筛选标签、主界面结构及全局说明 |
| **快速入门指南 (FAQ)** | `RenoDXCommander/MainWindow.FaqBuilder.cs` | 欢迎卡片、使用步骤 1~7、OptiScaler/驱动/Vulkan 专题说明及社区链接 |
| **游戏详情 - 核心组件** | `RenoDXCommander/DetailPanelBuilder.Components.cs` | ReShade、RenoDX、Luma、限制器等组件的安装状态、操作行与说明 |
| **游戏详情 - 扩展工具** | `RenoDXCommander/DetailPanelBuilder.Extras.cs` | ASI Loader、OptiScaler、RTX Encore、DLSS Enabler、20/30 FG、MFG 解锁及 dgVoodoo2 选项 |
| **游戏详情 - 神经渲染** | `RenoDXCommander/DetailPanelBuilder.NeuralRendering.cs` | DLSS5 Tool、DX11 桥接、ShortFuse、Feeder 模式说明、版本选择与状态标签 |
| **游戏详情 - DLSS/SL** | `RenoDXCommander/DetailPanelBuilder.Overrides.Dlss.cs`<br>`RenoDXCommander/DetailPanelBuilder.Overrides.NvidiaProfile.cs` | SR / RR / FG / Streamline 版本下拉菜单、预设、渲染比例及 DLL 部署按钮 |
| **游戏详情 - 驱动设置** | `RenoDXCommander/DetailPanelBuilder.Overrides.DriverSettings.cs`<br>`RenoDXCommander/Services/DlssPresetService.DriverSettings.cs` | 垂直同步（VSync）、撕裂控制、低延迟、平滑运动（Smooth Motion）、ReBAR、电源管理模式与 G-Sync 选项 |
| **游戏覆盖项 (Overrides)** | `RenoDXCommander/DetailPanelBuilder.Overrides.cs`<br>`RenoDXCommander/DetailPanelBuilder.Overrides.ShadersAddons.cs` | 游戏名称重命名、Wiki 排除（包含/排除）、位数（自动）、图形 API 覆盖及 DLL 命名规则 |
| **卡片按钮与状态文本** | `RenoDXCommander/ViewModels/GameCardViewModel.*.cs` | 游戏卡片上的主操作按钮（`安装`、`重新安装`、`更新`、`收藏`、`隐藏`）等动态文字 |
| **设置面板与全局选项** | `RenoDXCommander/SettingsHandler.cs`<br>`RenoDXCommander/MainWindow.Events.Settings.cs` | 设置页各模块开关、默认预设、缓存清理、NexusMods 授权与状态弹窗信息 |
| **弹窗服务与拖放交互** | `RenoDXCommander/DialogService.*.cs`<br>`RenoDXCommander/DragDropHandler.*.cs` | DLL 冲突覆盖警告、版本更新提示、拖放安装解压错误与确认弹窗 |

---

## 4. 本地审查与维护工作流程

在本地进行汉化调整或代码修正后，请按以下步骤执行本地只读代码审查：

### 第一步：扫描未汉化英文候选
在仓库根目录执行项目扫描工具：
```powershell
python tools/maintenance/audit_project.py --output-dir docs/maintenance
```
- **输出文件**：`docs/maintenance/english-review.tsv`
- **说明**：该工具采用启发式规则扫描 C# Tooltip、可见属性赋值与 XAML 文本。维护目标应保持待审英文候选为 0 或仅包含合理的专有名词。

### 第二步：运行 Roslyn 静态语法与结构审查
执行 C# 源码语法解析工具：
```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tools/maintenance/audit-source.ps1 -OutputFile docs/maintenance/source-audit.json
```
- **输出文件**：`docs/maintenance/source-audit.json`
- **说明**：该工具调用 .NET 8 SDK 内置的 Roslyn 编译器，对全部 320 个已跟踪 C# 文件进行无侵入式 AST 语法解析，确保所有修改无语法错误（`syntax_errors: []`）。

### 第三步：格式与换行符校验
检查工作区代码格式：
```powershell
git -c core.safecrlf=false diff --check
```
确保无未期望的尾随空格、空白行或格式异常。

### 第四步：推送代码触发云端构建
审查全部通过后，直接提交并推送到远程仓库：
```powershell
git add <修改文件>
git commit -m "feat/fix: 简要描述更新内容"
git push rhi-cn main
```
推送至 `main` 分支后，GitHub Actions 将自动启动 Windows 虚拟环境拉取代码并完成远程构建发布。

---

## 5. 安装包配置与 Windows 11 安全机制处理

### 5.1 Inno Setup 配置与语言包集成
- **脚本路径**：`RHI Setup.iss`
- **语言包**：根目录下的 `ChineseSimplified.isl`（基于官方 Inno Setup 6 简体中文标准语言包）。
- **配置实现**：
  ```ini
  [Languages]
  Name: "chinesesimp"; MessagesFile: "compiler:Default.isl,{#SourcePath}\ChineseSimplified.isl"
  ```
  通过直接引用仓库自带的 `ChineseSimplified.isl`，消除了云端编译环境缺少本地化语言文件的隐患，确保安装向导的每一个界面和按钮均稳定显示简体中文。

### 5.2 应对 Windows 11 智能应用控制 (Smart App Control)
- **问题成因**：
  Windows 11（22H2 及更新版本）的“智能应用控制 (SAC)”机制对于安装在系统级 `C:\Program Files` 目录且未购买商业付费数字证书（EV 代码签名）的卸载程序（`unins000.exe`）会执行严格拦截。
- **项目级优化措施**：
  在 `RHI Setup.iss` 中明确配置了优先非提权的用户模式安装：
  ```ini
  DefaultDirName={autopf}\{#MyAppName}
  PrivilegesRequired=lowest
  PrivilegesRequiredOverridesAllowed=dialog
  ```
  安装程序默认引导安装至当前用户目录（`%LocalAppData%\Programs\RHI`，与 VS Code、Chrome 用户级安装模式一致），无需管理员提权即可完成安装与卸载，极大避免了触发系统的安全拦截。

---

## 6. GitHub Actions 持续集成与发布规范

### 6.1 工作流文件
- **路径**：`.github/workflows/build-and-release.yml`
- **运行环境**：`windows-latest`

### 6.2 关键配置与环境变量
为规避 GitHub Actions 官方 Runner 在过渡到 Node 24 期间因插件版本引发的运行警告，顶层配置了环境控制变量：
```yaml
env:
  ACTIONS_ALLOW_USE_UNSECURE_NODE_VERSION: 'true'
```

### 6.3 资产发布规则
为给最终用户提供最纯净、直接的下载体验，工作流配置为**仅发布单个全量安装 EXE**：
- **生成产物**：`artifacts/RHI-vX.X.X-zh-CN.exe`（自包含、内置 DropHelper 辅助工具及全部 35 项配套着色器与配置文件）。
- **发布目标**：自动在 [GitHub Releases](https://github.com/XYZ-ZJ/RHI-CN/releases) 发布并打上版本标签。
- **免编译推送**：若某次提交仅为修改文档或微调注释，不希望消耗 GitHub 编译配额，可在提交信息中包含 `[skip ci]`，GitHub Actions 将自动跳过本次构建。
