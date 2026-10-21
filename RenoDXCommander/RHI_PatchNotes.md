⚠ **如果你之前已连接 GitHub，请在此次更新后重新登录** — 旧版本中的一个漏洞会导致过期会话无法被检测，该问题现已修复，但你存储的会话可能已经失效。

⚠ **登录 GitHub — 强烈建议** — RHI 下载的几乎所有内容都来自 GitHub：ReShade、RenoDX、OptiScaler、DLSS 版本、着色器包、Luma Mod、清单、更新检查等等。若不连接 GitHub 账户，这些操作将共享**每小时 60 次请求**的单一限额。这个额度很快就会耗尽，届时下载会静默失败 — 安装中断、更新检查被跳过、预下载显示为不可用，且没有任何明显原因。登录后限额提升至**每小时 5,000 次请求**。

连接方法：打开 **设置 → GitHub API**，点击 **Sign in with GitHub**。大约需要 30 秒。也可以粘贴个人访问令牌（PAT）— 无需任何权限范围，只需在 [github.com/settings/tokens](https://github.com/settings/tokens) 生成一个并粘贴进来即可。

## v2.8.7

### 新增

**OptiScaler F5 DLSSNR**
- 新增 F5 DLSSNR 作为 OptiScaler 的新变体，可在 OptiScaler ⚙ 齿轮的版本选择器中与 Stable、Nightly 和 DLSS NR 一起选择。
- 基于 janblade 的分支，支持多通道神经渲染，并内置 NR 转发器（`nvngx.dll_dlssnr.dll`）。需要单独预下载的 `nvngx_dlssnr.dll` 运行时（与 DLSS NR 变体相同）。
- 与其他 OptiScaler 变体一起自动更新，并显示在更新历史中。

### 变更

**OptiScaler 设置**
- 在 OptiScaler ⚙ 齿轮中 Deploy OptiScaler.ini 按钮上方添加了一条说明，阐明齿轮中的设置实时生效，部署按钮仅用于生成全新的 ini — 它不会写入已配置的设置。

### 错误修复

**OptiScaler Nightly**
- 修复了更新检查冷却期间 nightly 版无法自动更新到更新版本的问题。可用构建列表现在会在启动时和每次刷新时检查，更新检测不再依赖于先运行更新检查。
- 修复了点击 nightly 游戏上的更新时安装旧版本而不是下载新版本的问题。安装流程现在在标记更新时始终下载最新 nightly，并在下载后重新解析预下载目录。
- 当 Nightly 变体激活时，OptiScaler 行上的 Info 按钮和版本号现在链接到 nightly 的 GitHub 页面和发布页。

**RTX Encore**
- 修复了有新 beta 版可用时 RTX Encore 不自动更新的问题。更新检查现已接入标准组件更新流程，更新会显示在更新历史中。

**ReShade 截图 Discord 修复**
- 修复了 32 位插件未部署到 32 位游戏的问题。从 GitHub releases 下载时，若 `.addon32` 和 `.addon64` 都作为发布资产可用，RHI 现在会将两个文件都预下载。

**OptiScaler 检测**
- 修复了内部引用 OptiScaler 的工具（如 SCSKiller）被错误检测为 OptiScaler 安装的问题。二进制签名扫描现在仅在文件没有版本资源时运行 — 如果文件明确标识为其他工具，则不再被标记为 OptiScaler。

---

## v2.8.6

### 重要
- **此更新需要 Windows App Runtime 2.5.1。** 若尚未安装，安装器会自动下载并安装。该运行时更新包含 WinUI 3 的可靠性修复，可能会降低 UI 冻结的发生频率。

### 新增

**DLDSR 控制**
- 直接在 RHI 中启用或禁用 DLDSR（深度学习动态超分辨率）系数，无需打开 NVIDIA 控制面板。
- **DSR Smoothness 滑块** — 无需重新捕获即可调整平滑度（0–100%）。100 = 无锐化，数值越低锐化越强。DLDSR 的推荐范围是 55–80（DLDSR 本身比标准 DSR 锐利得多，所以需要比你预期的更高数值）。应用平滑度会像应用完整状态一样导致约 15 秒的黑屏。
- **Info 按钮** — 点击标题旁的 ⓘ 查看捕获和应用 DLDSR 状态的分步说明。
- **使用方法：**
  1. 打开 **NVIDIA 控制面板 → 管理 3D 设置 → DSR – Factors**
  2. 启用你想要的 DLDSR 系数（如 1.78x DL、2.25x DL）
  3. 在 RHI 设置中滚动到 **DLDSR 控制**，点击 **Capture Current** — 起个名字，如 "1.78x + 2.25x"
  4. 现在你可以在 NVIDIA 控制面板中禁用 DLDSR
  5. 每当需要恢复这些系数时，选择你保存的状态并点击 **Apply** — GPU 重启期间屏幕会黑屏约 15 秒，之后 DLDSR 生效
  6. 使用 **DSR Smoothness** 滑块并点击 **Set** 调整锐度/平滑度。这也会导致短暂黑屏 — 若想一次重启同时应用两者，请在点击 Apply 之前先设置。
- 便于开关 DLDSR，无需每次都进入 NVIDIA 控制面板
- 可保存多个配置（如 "Off"、"仅 1.78x"、"1.78x + 2.25x"）
- 需要管理员模式或以管理员身份运行 RHI 才能读写驱动注册表键

**Auto HDR（按游戏）**
- 从 RenoDX ⚙ 齿轮中按游戏强制开启 Windows Auto HDR，位于 RTX HDR 区下方。
- 适用于任何显卡 — 无需 NVIDIA App。也不需要 Windows 设置中的全局 Auto HDR 开关 — 按 exe 的注册表项本身就足够。
- 写入两个注册表位置：`HKCU\Software\Microsoft\Direct3D` 下的 `D3DBehaviors` 子键用于强制游戏进入 Auto HDR 管线，`HKCU\Software\Microsoft\DirectX\UserGpuPreferences` 下的 `AutoHDRStrength` 用于控制亮度 — 与 ledoge 的 autohdr_force 工具机制完全相同。
- 点击 RenoDX 行上的 **Configure Auto HDR** 打开设置对话框并调整亮度（0–100）。0 = 无增强，50 = 默认，100 = 最大（峰值约 1000 尼特）。
- 与 RTX HDR 互斥 — 启用其中一个会禁用另一个，启用任一个都会卸载当前激活的 RenoDX Mod。
- 更改在下次启动游戏时生效。

**OptiScaler Nightly 构建选择**
- 现在可以将游戏固定到特定的 nightly 版本，而不必始终使用最新版。打开 OptiScaler ⚙ 齿轮，使用新的 "Nightly Build" 下拉菜单 — 其中列出最近 30 个每日构建（约 2 个月的历史）。
- "Latest (auto-update)" 保持不变 — 游戏仍追踪最新 nightly 并照常自动更新。
- 固定版本会将该游戏冻结在该版本并排除在自动更新之外。当较新的 nightly 破坏某些功能时很有用。
- 固定的版本首次按需下载并在本地缓存以供后续安装。

**桌面快捷方式** *（开发中）*
- 右键点击游戏库中的任意游戏并选择 **创建桌面快捷方式**，即可在桌面放置快捷方式。
- 通过快捷方式启动游戏会经由 RHI — HDR 自动切换和分辨率自动切换的触发方式与应用内点击启动完全一致。
- 快捷方式图标使用游戏自身的图标：Steam、GOG、EA、Epic 和手动添加的游戏从游戏可执行文件提取。Xbox/Game Pass 游戏由于无法直接访问游戏可执行文件，从 Steam 或微软商店提取封面图 — Xbox 游戏的图标质量和可见度仍在改进中，将在未来更新中完善。
- 右键菜单还在原有的打开文件夹和查看备注之外新增了 **收藏** 和 **隐藏/显示**。

**RTX Encore**
- RTX Encore（SilyNoMeta）现在作为新的 Extras 行提供，位于 OptiScaler 和 DLSS Enabler 之间。
- 在支持 DLSS 帧生成的游戏中提供 X2–X6 帧生成、RTX 30 上的 Smooth Motion，以及 RTX 20/30/40 的神经渲染。
- 点击安装可选择代理 DLL 名称（version.dll、dinput8.dll、winmm.dll、dxgi.dll、d3d9/10/11/12.dll 等等 — ASI 加载器游戏可用 rtx-encore.asi）。已被其他 RHI 组件占用的名称会被禁用。磁盘上已有同名文件的名称以琥珀色显示 — RTX Encore 会备份并在卸载时恢复。
- ⚙ 齿轮可为神经渲染部署 310.8.0 版的 `nvngx_dlssnr.dll` — RTX Encore 只接受这个版本，拒绝其他任何版本。该文件与其他 NR 组件共享，仅在没有其他组件仍需要它时才会被移除。
- 与其他组件一起自动更新。

**RE Framework**
- 在 RE Framework 行上添加了 ⚙ 齿轮按钮，内含"删除 _storage_ 文件夹"按钮 — RE Framework 将此文件夹用作缓存并在每次启动时重建，因此删除它总是安全的。

### 变更

**Control Ultimate Edition — OptiScaler FG**
- 通过 Control 安装对话框安装 OptiScaler FG 时，RHI 现在会自动将游戏固定到 OptiScaler nightly 2026-10-04 版本。较新的 nightly 版本破坏了与 Control 的 DLSS 帧生成兼容性 — 这样做可确保开箱即用。

**GitHub 个人访问令牌**
- 在设置 → GitHub API 中新增 PAT 输入框，作为 OAuth 登录流程的替代方案。
- 粘贴任意经典 PAT（无需权限范围 — RHI 只读取公开仓库）。令牌保存在本地，除非你在 GitHub 上撤销，否则永不过期。
- 如果你不想使用 OAuth，或你的 OAuth 会话总是过期，这个功能很有用。

### 错误修复

**UI 冻结**
- RHI 现在约 5 秒内即可检测并从 UI 冻结中恢复，此前最多需要 60 秒。
- 修复了从睡眠中唤醒后自动重启被错误触发的问题。系统恢复后有 30 秒宽限期抑制重启检查。
- 修复了安装进行中触发自动重启的问题。若任何组件正在安装中，重启最多延迟 30 秒。
- 修复了快速切换游戏后反复冻结的问题。快速导航可能排队两次连续的面板重建，导致 UI 线程挂起。
- 修复了在选中游戏时打开设置面板可能出现的冻结。更改某些全局驱动设置（VSync、ReBAR）会触发与设置页打开冲突的面板重建。
- 修复了 RHI 静默吞掉本应发给 WinUI 而非系统托盘图标的内部框架消息（WM_APP/0x8000）的问题。这些消息现在被正确转发。
- 修复了多处打开对话框前在 UI 线程上读取 NVIDIA 驱动设置的问题。若 NVAPI 响应缓慢，打开配置 RTX HDR、多帧生成或 DXVK 设置可能导致 UI 阻塞长达 25 秒。现在会先在后台获取数值。
- 修复了更换 DLSS DLL 后在 UI 线程上刷新 DLSS 版本信息的问题，在慢速磁盘或有杀毒软件活动时可能导致 UI 卡顿。
- 修复了 RHI 在写入中途被强制结束时设置、游戏库和插件记录损坏的问题。这些文件现在以原子方式写入。
- 自动重启后，RHI 会重新选中冻结前打开的游戏，并在状态栏显示简短提示。

**DLSS/Streamline 检测**
- 新增 `dlssPathOverrides` 和 `streamlinePathOverrides` 清单字段 — 允许 RHI 在将 DLL 存放在递归扫描无法到达的非标准深层子目录中的游戏里找到 DLSS 和 Streamline DLL。The Sinking City 2 是首个使用此字段的游戏。

**DLSS/Streamline — The Sinking City 2**
- 修复了 The Sinking City 2 的 DLSS/Streamline 区域显示为空的问题。该游戏将 DLL 存放在 Engine 插件文件夹内 8 层深的位置，而不是可执行文件旁边。

**输出色彩设置**
- 修复了色彩深度和动态范围下拉菜单为空且无法点击的问题。NVAPI 显示器枚举对所有显示器都返回 "device not found"。

**OptiScaler 检测**
- 修复了 RTX Encore 的 `winmm.dll` 被错误检测为 OptiScaler 安装的问题。二进制签名扫描使用的通用字符串 "OptiScaler" 在 RTX Encore 中也作为引用出现。检测现在以文件描述元数据（`FileDescription = "OptiScaler"`、`CompanyName = "nitec"`）为主要检查，辅以使用更具体内部字符串的二次二进制扫描。

**赛博朋克 2077**
- 修复了安装按钮显示 "从 Discord 下载" 而不是使用常规 wiki/数据库 Mod 下载的问题。该游戏现在像其他 Mod 一样安装 RenoDX。

**随 Windows 启动**
- 修复了"随 Windows 启动"在每次启动时自动禁用的问题。设置面板在初始化期间移除了启动项。

**GitHub API**
- 修复了 GitHub 会话过期的用户看不到 RenoDX Mod 的问题。此前 RHI 会继续使用失效的登录令牌而不自知，导致 Mod、更新和下载静默失败，而设置页仍显示"已连接"。RHI 现在在启动时检查令牌有效性，若无效立即清除，并在状态栏显示提示，让你知道需要重新登录。

**进程不关闭**
- 修复了关闭窗口后 RHI 保持打开并占用 CPU 的问题。后台任务现在在关闭时停止，并有强制退出兜底，确保进程始终终止。

**外观**
- 修复了在 Windows 设置为浅色模式的系统上所有对话框以浅色主题渲染的问题。RHI 现在对每个对话框强制使用深色主题。

**DLDSR 控制**
- 当前状态显示现在显示显示器的友好名称（如 "LG ULTRAGEAR"、"MSI MPG343CQR"），而不是原始十六进制标识符。

**刷新**
- 修复了按下刷新后游戏详情面板变空白的问题。

**RenoDX**
- 修复了全新安装后已安装的 Mod 版本不显示、需要离开并返回该游戏才出现的问题。详情面板现在立即更新。
- 修复了 UE-Extended 游戏在 RenoDX 信息对话框中不显示 RHI 数据库中游戏专属备注的问题。

**MFG Ada Unlock**
- 修复了预下载文件被删除或缺失的用户更新检查跳过 MFG Ada Unlock 的问题。

**拖放 RenoDX Mod**
- 修复了将非标准名称的 `.addon64` 文件拖放到游戏上时，RHI 错误地提议删除现有 RenoDX Mod 的问题。

**Engine.ini**
- 修复了配置路径来自 PCGW 数据库且从未启动过的游戏在安装时不写入 Engine.ini 的问题。

**DLSS Fix**
- 修复了 DLSS Fix 每天多次作为更新下载的问题。版本检查期间的瞬时网络故障会返回未知结果，RHI 错误地将其视为新版本。现在版本检查无法解析时会跳过更新。

### 清单更新
- 为博德之门 3 添加了 `installPathOverrides`（`bin` 子文件夹）
- 为 The Sinking City 2 添加了 `dlssPathOverrides` — DLSS DLL 位于递归扫描无法到达的非标准深层子目录
- STAR WARS: Galactic Racer — 为 UE-Extended HDR 支持添加了 Engine.ini 路径。
- 巫师 3：狂猎 年度版现在安装与标准版相同的 RenoDX Mod。
- 镜之边缘：催化剂 — 取消了 wiki 关联。

---

## v2.8.5

*感谢 Lazorr 研究并贡献了本版本中的修复。*

### 错误修复

**可用 HDR Mod**
- 修复了列标题和 ✗ 标记过淡难以阅读的问题。

**关闭时的崩溃与冻结**
- 修复了不完整的关闭序列 — 定时器和面板扫描现在会停止，未完成的 UI 请求被取消，更新日志窗口关闭，OLE 拖放被清理，设置按正确顺序刷盘。
- 修复了进度对话框有时永久卡住的问题。对话框现在保留其模态槽位的所有权，直到 WinUI 完全完成关闭，包括错误路径。启动对话框（补丁说明、MOTD、更新检查）现在逐个运行，不再互相竞争。
- 修复了在安装器交接期间关闭到托盘时应用无法干净退出的问题 — 现在会显式退出，而不是依赖"关闭到托盘"行为。

**游戏库与刷新**
- 修复了在初始扫描仍在运行时启动刷新可能导致游戏库损坏的竞态条件。初始化和刷新现在串行执行，构建新库时保留旧库，UI 合并会等待后台扫描完成后再继续。

**单实例与管理员模式**
- 强化了单实例所有权，并将管理员任务查询改为带超时的异步操作。关闭信号文件现在会被清理，避免新启动的实例意外消费它。

**安装器**
- 移除了更新后将 RHI 带到前台时对 `AttachThreadInput` 的使用 — 这会把 RHI 的响应能力与安装器或其他前台应用绑定。替换为安全的基于消息的交接。

**下载**
- 为安装器下载添加了取消和 10 分钟超时，以及下载失败或取消时的部分文件清理。

---

## v2.8.4

### 错误修复

**GitHub 登录**
- 修复了启动竞态问题：所有网络请求在存储的 GitHub 令牌应用之前发出，瞬间耗尽未认证的 60 次/小时限额。令牌现在在任何请求发出之前应用。若令牌已被撤销，会在会话中被剥离并从设置中清除，下次启动生效。

**其他**
- 修复了某些启动时可用 HDR Mod 数量低于预期的问题。Luma release Mod 在 GitHub 获取完成前就被合并，所以显示的数量取决于当时加载了多少。计数现在始终稳定。

---

## v2.8.3

### 错误修复

**冻结**
- 修复了选择装有 DLSS 的游戏后持续 UI 冻结的最常见原因。若游戏文件夹被删除或出现权限错误，内部扫描锁不会被释放，导致后续每个 DLSS 和驱动设置面板无限挂起。

**着色器包**
- 修复了清除着色器缓存后大多数着色器包不下载的问题。启动时两个下载任务竞争，导致包在下载中途被跳过且从未解压。

**GitHub 登录**
- 修复了被撤销的 GitHub 登录令牌导致每个请求都出现 401 错误的问题。RHI 现在在启动时检测被撤销的令牌并自动清除，回退到未认证访问。

**其他**
- 修复了 reshade.me 返回服务器错误时 ReShade 不下载的问题。他们的服务器间歇性地返回 HTTP 500，即使页面正确加载。RHI 现在无论错误代码如何都会读取页面。
- 修复了 OptiScaler 预设不保存 6 项设置的问题：OptiScaler 版本、Upscaler API、Upscaler、FG Enabled、Force Reflex 和 Use Games Reflex Markers。
- 修复了巫师 3：狂猎 重制版（以及其他潜在游戏）中第二个较旧的 RenoDX 插件在每次重启后重新出现的问题。
- 修复了可用 HDR Mod 按钮显示的计数因点击时机不同而不一致的问题。按钮现在在扫描完成前禁用。

### 维护
- 添加了详细的诊断日志，帮助调查剩余的 UI 冻结报告。

---

## v2.8.2

### 新增
- **捐赠按钮** — 工具栏中帮助和设置之间的新按钮。打开一个对话框，列出所有 Mod 作者及其作品以及他们的 Ko-fi 页面直达链接。作者按字母排序，通过清单更新，因此新条目无需更新应用即可显示。

### 错误修复

**冻结**
- 修复了选择某些游戏（尤其是装有完整 DLSS 配置 — SR、RR、FG 和 Streamline 全部安装）时出现的 UI 冻结。冻结可能无限持续并需要强制结束应用。5 秒安全超时现在防止其永久阻塞。
- 修复了在 SR、RR 或 FG 版本设为 NVIDIA Override 的游戏上快速应用无反应的问题。现在会正确禁用覆盖并部署所选版本，与手动更改下拉菜单相同。

**插件**
- 修复了 Unity 插件（及其他双位数插件）在会话之间被静默从 64 位换成 32 位的问题。后台扫描检测到不同位数时，RHI 会移除 64 位文件并替换为 32 位版本。现在两个版本独立保留。
- 修复了重启应用或安装 Mod 后 MFG Ada Unlock 从游戏文件夹中被移除的问题。RHI 错误地将其视为非托管插件并清理掉了。
- 修复了插件更新不部署到游戏文件夹的问题。新版本下载后（如 MFG Ada Unlock），只在预下载目录中更新，而游戏文件夹中的文件保持旧版本，直到手动重新安装。

**其他**
- 修复了 OptiScaler Nightly 安装失败（如因 GitHub 限流）时在游戏文件夹中留下 Streamline 和 DLSS Enabler 文件的问题。安装无法完成时这些文件现在会被正确清理。
- 修复了配置 GitHub API 令牌后来自 Lyall 的超宽修复链接不加载的问题。令牌被发送到了错误的服务器而被拒绝。

---

## v2.8.1

### 新增
- **Control Ultimate Edition — OptiScaler FG** — 安装对话框新增两个选项："Install OptiScaler FG" 一键为帧生成配置 OptiScaler Nightly（Streamline、DLL 重命名和 INI 设置全部自动处理），"Using HDR?" 切换 RenoDX 预设中的 FgSlHdr10 标志。
- **dgVoodoo2** — Extras 区现在为任何 DX9 游戏提供独立安装。将 DX9→DX11 转换层部署到游戏文件夹，使 ReShade 计算着色器和 DLSS5 Feeder 可以在老游戏上运行。可与 Luma 和 Feeder 安全共存。⚙ 齿轮可选择要部署的版本。
- **游戏卡上的 Luma Mod 信息** — Luma 安装后，已安装的插件文件名（如 `Luma-Prey.addon`）现在与 RenoDX 插件一起显示在徽章行中。RenoDX 和 Luma 的 Mod 作者显示为独立徽章（如 RenoDX: Marat · Luma: Pumbo）。安装后卡片立即更新，无需刷新。
- **卡片布局** — 商店和引擎徽章现在与启动按钮同行右对齐。图形 API 和位数徽章与已安装 Mod 名称同行右对齐。
- **GitHub API** — 设置中的新区块。登录 GitHub 可将 API 限额从每小时 60 次提升到 5,000 次。提高更新检查、清单获取和 DLSS 版本查询的可靠性。令牌在启动时自动存储并应用。
- **后台更新检查** — 更新区块中的新设置。"On"（默认）保持现有行为。"Minimal" 停止所有后台组件检查 — 仅清单、PCGW 数据和 DLSS 版本自动获取。适合喜欢手动检查更新的用户。

### 变更
- **游戏覆盖折叠摘要** 现在一目了然地显示 RS Channel、按游戏的着色器/插件模式和任何 Update All 排除项。
- **DLSS / Streamline 折叠摘要** 现在在非默认时显示激活的预设字母和渲染缩放。
- **着色器包更新日志** 现在为有 GitHub releases 的包（Lilium HDR Shaders、PumboAutoHDR、UltrawideSideGlass）显示真实版本号。
- **Streamline 部署** 现在在 OptiScaler/Streamline 子文件夹中包含 nvngx_dlssg.dll 以支持 FG。
- **OptiScaler Nightly FG 齿轮** — 三项新设置：FG Enabled、Force Reflex 和 Use Games Reflex Markers。

### 错误修复

**冻结**
- 修复了影响多个版本中拥有完整 NVIDIA 驱动配置（DLSS + Streamline + ReBAR）游戏的 UI 冻结。根本原因是多个后台任务对 NVIDIA 驱动会话对象的无同步并发访问 — 面板读取和用户触发的写入相互竞争并损坏驱动状态，永久阻塞 UI。所有 NVAPI 会话访问现在通过单一锁串行化。
- 修复了第二条冻结路径：设置面板在初始化完成前将 NVIDIA 驱动值写回，与游戏面板读取相同值发生冲突。
- 修复了选择装有神经渲染的游戏（如 Control、巫师 3）时的冻结。
- 修复了 Extras 和神经渲染区块溢出容器并将按钮挤出屏幕的问题。

**dgVoodoo2 + ReShade 共存**
- 修复了在已有 ReShade 位于 d3d9.dll 的游戏上安装 dgVoodoo2 时共存失败的问题。ReShade 现在被正确移动到 dxgi.dll，从而挂钩 dgVoodoo2 的 DX11 输出。卸载时无需完整重装 ReShade 即可清理。部署的文件现在为小写 `d3d9.dll`，与原始文件一致。

**其他**
- 修复了 DXVK "Deploy dxvk.conf" 按钮始终部署通用配置的问题。Lilium HDR 现在会部署正确的预设专属内容。
- 会话日志现在在写入磁盘前对用户名、Steam 用户 ID 和 Xbox 包名进行遮蔽。
- 修复了 PCGW 数据中 UE4/5 配置路径指向具体文件名而非文件夹的问题，该问题导致数百个游戏的 Engine.ini 部署静默失败。

### 清单更新
- 修复了 Saints Row 2、Bye Sweet Carole 使用错误 ReShade 位数的问题。
- 修复了刺客信条：大革命使用 32 位 ReShade 的问题。
- 修复了 ReShade 在 NTE: Neverness To Everness 上不加载的问题 — 添加了 d3d12.dll 名称覆盖。
- 为巫师 3：狂猎 重制版添加了安装警告（可用的 RenoDX Mod 面向原版）。

---

## v2.8.0

### 新增
- **NVIDIA 面板拆分** — "Nvidia Profile Overrides" 区块现在拆分为两个独立面板：**DLSS / Streamline**（SR、RR、FG、SL 版本和预设）和**驱动设置**（VSync、低延迟、Smooth Motion、电源/G-Sync、ReBAR）。各自拥有独立的折叠/展开、拖动排序手柄和折叠摘要行。各自记住自己的折叠状态。保存过旧区块位置的用户会在该位置自动看到两个新区块。
- **组件更新历史现包含 RenoDX Mod 更新** — 通过 Update All、下载监视器和拖放应用的安装和更新现在都会记录到更新日志中。每个条目显示被替换的版本和替换后的版本。

### 变更
- 最小窗口宽度设为 1220px。
- 状态徽章（Working / May Work / WIP）已从 RenoDX 信息对话框中移除 — 相同的信息已显示在游戏卡上，重复展示没有必要。
- 安装警告对话框现在将 URL 渲染为可在浏览器中打开的可点击链接。

### 错误修复

**Update All**
- 修复了 Update All 每次运行都重新下载并重装每个 RenoDX Mod 的问题，即使 Mod 已是最新版本。现在只安装被标记为有实际更新的 Mod。

**OptiScaler**
- 修复了 OptiScaler 卸载删除游戏 `plugins\` 文件夹的问题 — 这会清掉赛博朋克 2077 上的 Cyber Engine Tweaks、RED4ext 等游戏 Mod。卸载现在完全跳过根 `plugins\` 文件夹；`OptiPatcher.asi` 仍通过专门的步骤正确移除，该步骤只删除该特定文件，仅在文件夹之后为空时才移除文件夹。

**文件监视器**
- 修复了在游戏选择对话框上点击取消时 RenoDX 和 Luma 压缩包被从下载文件夹删除的问题。压缩包不再自动删除 — 只有直接的 `.addon` 文件在成功安装后才被移除。

**DXVK**
- 修复了 DXVK 安装后未将 DX10/DX11 游戏注册为使用 Vulkan ReShade 的问题。游戏卡现在正确切换到 Vulkan 模式，徽章更新，着色器部署，且状态在重启后保持 — 与现有 DX9 行为一致。卸载现在正确恢复 ReShade 作为 DX 代理并重新部署着色器。

**UI**
- 修复了当选中具有大型 NVIDIA 驱动配置的游戏（如质量效应：仙女座）时点击检查更新后 UI 冻结 30–60 秒的问题。设置面板打开期间，NVIDIA 配置面板现在跳过重建，因为它不可见且昂贵的布局工作毫无意义。
- 修复了应用更新下载期间当选中装有 DLSS SR+FG+Streamline 的游戏（如战神：诸神黄昏）时 UI 冻结的问题。任何对话框打开期间，NVIDIA 配置和神经渲染面板现在跳过重建 — 用户正在看对话框，而不是游戏面板。
- 修复了点击装有完整 DLSS 的游戏（SR+RR+FG+Streamline，如 Control、生化危机 4、战神：诸神黄昏）时永久 UI 冻结（需要任务管理器结束）的问题。根本原因：当带星号列的 `Grid` 嵌套在 `ScrollViewer` 内的 `StackPanel` 中时，WinUI 3 进入无限布局循环 — 已知的引擎缺陷。通过将 NVIDIA Profile 和 DLSS 网格中的所有星号列定义替换为计算的固定像素宽度修复。

### 清单更新
- 为巫师 3：狂猎 重制版添加了安装警告 — RHI 中可用的 RenoDX Mod 面向原版；警告链接到重制版专属 Mod 的 Discord 帖子。
- 修复了 Bye Sweet Carole 安装 32 位 ReShade 的问题 — 已添加到 64 位覆盖列表。
- 修复了 ReShade 在 NTE: Neverness To Everness 上安装不正确的问题 — 添加了 DLL 名称覆盖（`d3d12.dll`）。
- 修复了 Saints Row 2 安装 64 位 ReShade 的问题 — 已添加到 32 位覆盖列表。
- 修复了刺客信条：大革命安装 32 位 ReShade 的问题 — 已添加到 64 位覆盖列表。

---

## v2.7.9

### 新增
- **组件更新历史** — 底部栏新增"更新"按钮，打开 RHI 已下载所有内容的带日期日志：ReShade、RenoDX 插件、着色器包、OptiScaler、ReLimiter、Display Commander 等等。条目按日期分组（今天 / 昨天 / 完整日期），每条显示旧版本和新版本。历史跨会话保留，可随时清除。
- **Unity 游戏专属设置** — 为 Unity 引擎游戏安装 RenoDX Mod 时，RHI 现在写入正确的按游戏 INI 设置。渲染目标升级、交换链格式等兼容性键根据 RHI 数据库自动应用，无需手动配置。
- **DLSS Tool ZZZ 加载顺序** — 神经渲染区（仅 DLSS Tool 方法）的新开关，可将 DLSS Tool 插件部署为 `zzz_renodx-dlss.addon64`，使其在 ReShade 插件顺序中最后加载。开或关会立即重命名磁盘上的文件。RHI 将两个文件名识别为同一插件 — 跟踪、自动更新和版本切换在任一名称下都无缝工作。
- **Control Ultimate Edition** — 安装 Control RR Mod 现在自动处理一切。RHI 升级 DLSS、部署 Ray Reconstruction 运行时、纠正 renderer.ini 中的 HDR 预设，并清除游戏 NVIDIA 驱动配置中设置的 DLSS SR 预设。确认前会显示将要发生的操作说明（以及这不是 HDR Mod 的提示）。

### 变更
- Unity 游戏现在在已安装插件旁显示 ✓ 或 🔨 状态图标，与 UE-Extended 和命名 Mod 游戏的行为一致。
- 在设置中更改全局着色器设置（Off / RHI 托管 / Custom）现在立即对所有游戏生效 — 无需手动刷新。
- RenoDX ⚙ 齿轮的兼容性设置现在为每个键显示正确的命名选项 — Blit Copy Hack、Copy Destinations、Swapchain Format、Tonemap/Scaling Offset、Proxy Revert State 和 Swapchain Compat。升级路径和 Engine.ini HDR/LUT 设置已移至 UE-Extended 设置区，与尼特值并排，两列之间有垂直分隔线。

### 错误修复

**着色器**
- 修复了全局着色器 "Off" 设置覆盖按游戏着色器覆盖的问题。设有按游戏 Custom 或 Select 覆盖的游戏现在即使在全局设置为 Off 时也会收到其配置的着色器。无覆盖的游戏继续像以前一样继承 Off 设置。
- 修复了选择内置着色器包时自定义着色器文件被自动勾选的问题。依赖扫描器将标准 ReShade 头文件与用户自定义文件夹中的副本匹配了。

**DXVK**
- 修复了游戏装有 Lilium HDR 但未显式存储变体时 DXVK 更新到错误变体的问题。安装或更新后现在始终按游戏保存已安装的变体。现有的 Lilium HDR 安装在首次启动时自动纠正。
- 修复了 DXVK 安装期间写入的 reshade.ini 缺少叠加层、截图和快捷键区块的问题。截图保存路径和快捷键没有从设置中继承过来。

**神经渲染**
- 修复了 NR 插件版本下拉菜单在 1 小时缓存过期前不显示新版本的问题。完全刷新现在绕过缓存并立即获取当前版本列表。
- 修复了选择 "Latest" 且新版本发布时 DLSS Tool 不在游戏文件夹中自动更新的问题。自动重新部署通道跳过了 NR 管理的文件，因为标准插件部署系统有意不跟踪它们。
- 修复了 NR 插件版本下拉菜单中 rc10 排在 rc5 之下的问题。

**ReShade**
- 修复了使用 Custom ReShade 渠道的游戏每次启动都显示错误更新通知的问题。更新检查现在始终使用当前的按游戏渠道，而不是安装时记录的渠道。

**Unity**
- 修复了 Unity 引擎游戏的信息对话框中不显示 RenoDX 数据库中游戏专属评论的问题。

**其他**
- 修复了通过自定义 exe 覆盖（或自动检测的 exe 回退）启动的游戏因"数据目录缺失"等类似错误而无法启动的问题。工作目录没有被设置为游戏文件夹。
- 提高了选择装有完整 DLSS 安装的游戏（SR、RR、FG、NR 和 Streamline — 如潜行者 2、刺客信条：影）时的稳定性。NVIDIA Profile 面板现在在提交到可视化树之前预先测量其布局，降低 UI 冻结的可能性。

### 清单更新
- 为巫师 3：狂猎 重制版添加了 Nexus Mods 链接和安装路径。

## v2.7.8

### 新增
- DXVK 现在可以安装在 DirectX 11 游戏上。所有三个变体（Development、Stable、Lilium HDR）都完整支持 DX11 — DX11 部署路径早已存在，只是开关被隐藏了。Lilium HDR 使用其 DX11 专属的预设系统（交换链和渲染目标升级）。同时检测到 DX12 或 Vulkan 的游戏仍被排除。
- 要在同时检测到 DX12 的 DX11 游戏上使用 DXVK（PE 导入表同时包含两者的虚幻引擎游戏很常见），请在游戏覆盖中将图形 API 覆盖设为 DirectX 11 — 这会告诉 RHI 将游戏视为仅 DX11，并使 DXVK 开关可用。

### 错误修复

**OptiScaler**
- 修复了在变体字段添加之前创建跟踪记录的游戏上，OptiScaler 自动更新使用错误变体（Stable 而非 Nightly）的问题。记录中没有存储变体时，更新现在会交叉引用按游戏的变体设置，因此 Nightly 游戏始终从 Nightly 预下载目录更新。

**其他**
- 修复了最小化到系统托盘时双击桌面快捷方式导致 RHI 关闭而不是恢复的问题。原因是 Windows API 声明中的 DLL 名称错误（`GetCurrentThreadId` 从 `user32.dll` 而不是 `kernel32.dll` 导入），在某些系统上抛出异常并使运行中的实例崩溃。
- 修复了 ASI Loader 未安装时在 Extras 头部显示为已安装的问题 — 跟踪的 DLL 名称被持久化了但安装记录已消失。过期条目现在自动清除。

## v2.7.7

### 变更
- 来自 RHI 数据库的命名 Mod 现在在游戏头部显示绿色 ✓ 或 🔨 状态图标，与 UE-Extended 游戏相同。
- Unity 引擎兼容性设置（Swapchain Proxy、Swapchain Encoding、Force Pipeline Cloning、Force Borderless 等）现在在使用它们的游戏的 RenoDX ⚙ 齿轮兼容性设置区中显示。

### 错误修复

**冻结与窗口行为**
- 修复了在游戏之间导航时随机 UI 冻结的问题。后台扫描持有阻塞 NVIDIA 配置区块加载的锁。
- 修复了快速滚动浏览有 NVIDIA 驱动配置的游戏时 UI 冻结的问题。
- 修复了启动时 RHI 在其他窗口后面打开的问题。
- 修复了从系统托盘恢复或被第二个实例打开时 RHI 不回到前台的问题。
- 修复了最大化窗口重新启动时不以最大化状态恢复的问题 — 它会显示为无边框但实际上并未最大化。

**PCGW 链接与引擎徽章**
- 修复了检测到的名称中带有商标符号（™、®、©）但 PCGamingWiki 数据库中省略的游戏不显示 PCGW 链接和引擎徽章的问题 — 如 Deus Ex: Mankind Divided™。
- 修复了 Steam 使用直撇号而 PCGamingWiki 数据库使用弯撇号的游戏不显示 PCGW 链接和引擎徽章的问题 — 如刺客信条系列。
- 修复了文件夹名称省略副标题冒号的 EA App 游戏不显示 PCGW 链接和引擎徽章的问题 — 龙腾世纪：审判和质量效应：仙女座现在正确解析。
- 修复了 DX9 误报游戏（如 CrossCode）不显示引擎徽章的问题 — 导入旧版 D3D 垫片的 NW.js 和 Electron 游戏被标记为 DX9。PCGW 数据现在纠正了这一点。

**DXVK**
- 修复了 DXVK 默认使用 Development 变体而不是 Lilium HDR 的问题。
- 修复了安装了 DXVK 的 DX9 游戏不出现在 DX9 搜索中的问题。卡片现在正确显示 "DX9 / VLK"。
- 修复了在 DX9 游戏（如质量效应、暗黑破坏神）上安装 DXVK 后 ReShade 显示为未安装的问题。
- 修复了卸载 DXVK 后不刷新时 ReShade 卸载按钮无反应的问题。
- 修复了卸载 DXVK 后 VLK 徽章残留的问题。
- 修复了在尚未安装 ReShade 的情况下安装 DXVK 时着色器不部署的问题。

**ReShade**
- 修复了将游戏的图形 API 覆盖切换为 Vulkan 时不移除 DX ReShade DLL 的问题。过渡时现在自动卸载 DLL，反之亦然。
- 修复了 Vulkan ReShade 安装/卸载状态不立即更新面板的问题。
- 修复了安装或卸载后状态栏中的 ReShade 安装计数不立即更新的问题。
- 修复了 Vulkan 和 Unity 游戏上 ReShade ⚙ 齿轮变灰的问题。

**OptiScaler**
- 修复了每次打开 OptiScaler ⚙ 齿轮时 DLSS NR 和 Nightly 变体选择被静默重置为 Stable 的问题，该问题导致安装和更新使用错误版本。

**神经渲染**
- 修复了新 "Latest" 版本发布时 DLSS5 Tool 和 ShortFuse DLSS Tool 不重新部署到游戏文件夹的问题。部署跟踪器缓存在安装后从未刷新。
- 修复了选择 "Latest" 时新 DLSS5 Tool 版本不自动预下载的问题。文件在 4 小时更新检查运行前不会被下载。
- 修复了 Feeder 首次启动时因"找不到目录"错误而安装失败的问题。

**其他**
- 修复了将 RenoDX 插件拖放到同一游戏上时 MFG Ada Unlock 被移除的问题。
- 修复了按文件夹名检测的游戏（如 Avatar: Frontiers of Pandora 被检测为 "AFOP"）上命名 Mod 不显示 ✓/🔨 状态图标的问题。
- 修复了点击 NVIDIA 驱动配置从未扫描过的游戏时多秒延迟的问题。
- 修复了 ReShade 渠道设为 "No Addons" 时游戏覆盖中的着色器下拉菜单被禁用的问题。
- 修复了 Swapchain Encoding 标签显示 "Gamma" 而不是 "Linear" 的问题。

### 维护
- 来自 PCGamingWiki 的引擎名称现在包含在集中的 `pcgw_data.json` 数据库中。当 PE 扫描无法识别引擎时（Game Pass 和 EA App 安装中很常见），引擎徽章现在从 PCGW 数据填充，覆盖数据库中全部 55,000+ 游戏。

### 清单更新
- 修复了骑马与砍杀 2：霸主的安装路径 — RHI 之前检测到错误的子文件夹。
- 修复了只狼：影逝二度不显示其 Luma Mod 的问题。
- 修复了 Arma Reforger NVIDIA 配置指向错误 exe 的问题。
- 为 CONTROL Resonant 和最终幻想 XV WINDOWS EDITION 添加了 Nexus Mods 链接。
- 添加了 UltrawideSideGlass 着色器包 — 用缩放/镜像/磨砂玻璃效果填充超宽黑边。

## v2.7.6

### 新增

- **可用 HDR Mod** — 快速开始旁的新按钮。打开所有受支持游戏的可搜索列表，显示 RenoDX 和 Luma 中哪些可用，以及直达下载链接。
- **DLL 命名覆盖重新设计** — 旧的启用/禁用开关已移除。ReShade、Display Commander 和 OptiScaler 在游戏覆盖面板中各自拥有独立的下拉菜单。选择名称会立即重命名文件；选择 `--------` 恢复。重置按钮一次性恢复全部三个。
- **折叠区块现在显示实时摘要** — 详情面板区块折叠时，关键信息内联显示：已安装的组件版本（组件）、激活的 DLSS/Streamline 版本（NVIDIA Profile）、激活的 NR 方法（神经渲染）、已安装的扩展（Extras）和任何激活的覆盖（游戏覆盖）。
- **来自 GitHub release 资产的 Luma Mod** — 已作为构建发布但尚未在 Luma wiki 上发布的 Mod 现在自动出现在 RHI 中。
- **PCGamingWiki 数据现在作为单一文件加载** — 取代按游戏请求。覆盖 55,000+ 游戏。PCGW 链接、API 检测和 Engine.ini 路径都照常工作，只是更快。

### 神经渲染

- **安装时切换插件版本** — 神经渲染已安装时更改版本下拉菜单现在会就地替换插件文件。无需先卸载。只替换插件文件 — DLSS DLL、配置和着色器保持不变。
  - DLSS5 Tool / DLSS5 Tool + Bridge：替换 `renodx-dlss5.addon64`
  - ShortFuse DLSS Tool：替换 `renodx-dlss.addon64`
  - Feeder：独立替换神经消费者（`renodx-dlss5.addon64`）和/或 Feeder 插件本身（`dlss5-feed.addon64`）
  - Bridge：替换 `dlss5-bridge.addon64`
- **NR DLL 版本选择** — NR DLL 版本下拉菜单现在对所有四种 NR 方法都激活（此前仅 DLSS5 Tool、Bridge 和 Feeder）。ShortFuse DLSS Tool 现在可以在安装前或安装期间指定特定的 NR DLL 版本。安装 NR 后更改版本会为所有方法就地替换 DLL。
- **版本下拉菜单显示最新版本号** — "Latest" 现在在括号中显示实际版本，如 `Latest (310.8.2 (20/30/40/50))`。

### 变更

- 移除简单视图 — 应用始终处于详情视图。
- DLSS5 Tool、ShortFuse DLSS Tool、DLSS5 Feeder、DX11 Bridge 和 MFG Ada Unlock 已从插件选择器中移除 — 请改用神经渲染和 Extras 区块。现有的按游戏选择被静默清理；不会卸载任何内容。
- 组件区块添加了"HDR Mods"分隔符。
- OptiScaler FG Nvngx Override 下拉菜单添加了"Combo"。
- OptiScaler DLSS RR 预设下拉菜单添加了预设 F。
- 可用 HDR Mod 对话框现在固定宽度 — 此前对话框在滚动列表时宽度会轻微偏移。

### 错误修复

**UI 响应性**
- 修复了导航、安装、卸载和菜单交互期间的 UI 冻结。将 20+ 个阻塞操作移出 UI 线程：异步日志记录、游戏卡上的缓存文件系统状态、防抖的设置保存、异步 7-Zip 解压、异步批量部署循环、异步齿轮对话框读取、异步自动更新通道、异步设置页初始化等等。
- 修复了选择某些装有神经渲染的游戏时 UI 冻结的问题 — NR 状态面板在 UI 线程上进行多次文件系统读取（File.Exists、每个 DLL 的 GetFileVersion）。这些现在在 UI 更新前于后台线程预计算。
- 修复了 RHI 启动最小化到托盘（如 Windows 启动时）时窗口大小和位置不恢复的问题。从托盘显示时窗口现在以正确的大小和位置打开。

**PCGW 可靠性**（感谢 kaeldrin-gh）
- 修复了单次超时或限流错误后 PCGamingWiki 查询在会话剩余时间内永久失败的问题。服务现在暂时暂停并在恢复后自动重试。尊重 429 响应上的 `Retry-After` 头。
- 修复了瞬时 PCGW 故障被持久化为永久"无结果"缓存条目的问题，该条目会在服务恢复后仍抑制游戏的 PCGW 链接。

**神经渲染**
- 修复了 `DLSS5_Feed.fx` 和 `lumenite_Kernel.fx` 在无先前着色器选择的游戏上不部署的问题（如全新安装时的龙腾世纪：审判）。着色器选择现在在 ReShade 安装步骤运行之前写入。
- 修复了 `renodx-mfgunlock.addon64` 被检测为 RenoDX HDR Mod 的问题 — 现在被正确排除在游戏专属插件扫描之外。

**启动挂起**
- 修复了装有 RTX Remix 的游戏（如辐射：新维加斯）的用户在"Building cards..."上无限挂起的问题。RTX Remix 创建的循环目录符号链接导致 DLSS 扫描器永远跟随。扫描器现在在深度 8 处停止，并在路径超过 300 个字符时放弃。

**睡眠唤醒冻结**
- 修复了唤醒后当选中装有 DLSS 或驱动配置设置的游戏时应用冻结的问题。NVAPI 读取现在以 5 秒超时运行。

**其他**
- 修复了仅 Nexus 的 Mod 不显示"新 Mod"通知的问题。
- 修复了在 ShortFuse DLSS Tool 或 Feeder 游戏上清除全局插件选择器后游戏文件夹中残留 `renodx-dlss5.addon64` 文件的问题。
- 修复了安装 DLSS NR 变体时 OptiScaler 版本/Info 按钮打开错误发布页的问题。
- 修复了 RTX 40 MFG Unlock 和 MFG Ada Unlock 安装或卸载后行状态不立即更新的问题 — 此前需要刷新。
- 修复了 Unity 插件游戏（AI Limit、Death's Door、Outer Wilds 等）每次安装都重新下载插件而不是使用缓存文件的问题。插件 URL 现在被解析为 GitHub Releases（返回可靠的文件大小），无论 wiki 或清单提供什么 URL。

### 清单更新

- 添加了 dgVoodoo2 v2.87.5（作者专门发布以避免 Defender 对 D3D9.dll 的误报检测）。
- 为 21 个已作为构建发布但尚未上 wiki 的 Luma Mod 添加了安装警告。
- 为 Luma release 资产匹配添加了 `L.A. Noire` 名称映射。
- 添加了守望先锋安装子路径（`_retail_`）。
- 添加了 SILENT HILL: Townfall 引擎提示（UE 5.6.1）和安装子路径。
- 为 CONTROL Resonant 添加了引擎提示（Northlight Engine）。

## v2.7.5

### 错误修复

**设置**
- 修复了 PC 空闲或 GPU 从睡眠唤醒后打开设置时应用冻结的问题。NVAPI 读取现在在后台线程上以 5 秒超时运行，因此设置页始终立即打开。

**DLSS5 Feeder**
- 修复了 API 扫描返回空结果集的 DX9 游戏（哥特王朝 2、暗黑破坏神等）上不部署 dgVoodoo2 的问题。DX9 检测现在回退到主要检测到的 API，因此 dgVoodoo2 在所有 DX9 游戏上正确安装。
- 修复了未检测为 DX9 的 32 位游戏（如 Diablo GOG）上不部署 host64\\ 文件夹的问题。所有 32 位 Feeder 安装都需要 host64\\ 文件夹 — 不再依赖于检测到 DX9。
- 修复了重启 RHI 后 LumeniteFX 和 DLSS5_Feed.fx 从游戏文件夹中消失的问题。着色器选择在 UI 线程上保存，可能被并发的设置保存覆盖 — 现在在安装期间同步写入。
- 修复了固定特定 Feeder 版本时 64 位 Feeder 插件被复制到 32 位游戏上的问题。版本化预下载只存储 `.addon64`，因此 32 位游戏现在始终使用具有正确 `.addon32` 的 AddonPackService。

**Luma**
- 修复了在最初通过拖放安装 Luma 的游戏上点击 "Install Luma" 时崩溃的问题。这些游戏没有下载 URL，因此 RHI 现在显示"将 Luma 压缩包拖放到卡片上以重新安装"而不是崩溃。

### 清单更新

- 修复了 Clive Barker's Hellraiser: Revival Demo 的 Engine.ini 写入错误文件夹的问题（写到了 `Hellraiser` 而不是 `Hellraiser_Demo`）。

## v2.7.4

### 错误修复

- 修复了着色器预下载文件被删除后 `DLSS5_Feed.fx` 不部署的问题 — RHI 现在自动从缓存的 Feeder zip 重新解压。阻止重新解压的过期注册条目也会在启动时清除。

### 清单更新

- 为骑马与砍杀 2：霸主添加了引擎提示（Daroya Engine）。

## v2.7.3

### 新增

- UE-Extended 游戏现在在插件名称旁显示状态图标 — 标记为完整的 Mod 显示绿色 ✓，仍在进行中的显示 🔨。
- 现在可以在着色器选择器中从自定义着色器文件夹选择单个文件。来自 `%LocalAppData%\RHI\reshade\Custom\Shaders\` 和 `\Textures\` 的文件作为"自定义着色器"区块显示在推荐和扩展包之间，按子文件夹分组。勾选或取消勾选单个文件以精确控制部署内容。配置面板中的"打开自定义文件夹"按钮可直接打开该文件夹。
- DXVK 已移至 Extras 区，位于新的"API 升级"子标题下。安装按钮在符合条件的游戏（DX8/9/10）上始终可用。变体选择（默认 Lilium HDR、Development、Stable）和 Lilium 预设现在位于 DXVK 齿轮中，与现有的呈现方法设置并列。

### 神经渲染

- Feeder 和 Bridge 版本选择 — 现在可以固定 Feeder 或 Bridge 插件的特定发布版本，而不必始终使用最新版。下拉菜单显示完整的发布历史（包含 beta 在内 28+ 个版本）。
- ShortFuse（DLSS Tool）现在在除 OpenGL 外的所有 64 位游戏上可用 — 此前只在有原生 DLSS 的游戏上显示。
- 修复了 Feeder 在 64 位 DX9 游戏上安装错误 dgVoodoo2 文件的问题 — 始终使用 32 位版本，导致 dgVoodoo2 无效且着色器编译失败。在受影响的游戏上重新安装 Feeder 即可修复。
- 修复了安装 Feeder 时全局着色器不从游戏文件夹中立即移除的问题。
- 修复了 `DLSS5_Feed.fx` 安装时不部署到游戏文件夹的问题。
- 修复了神经渲染面板在某些会话中不构建的问题。

### 游戏检测

- Unity 游戏现在基于 Unity 自身的配置文件正确报告其 API，而不是 PE 导入扫描（后者读取 Unity player DLL 并看到所有 API）。这修复了 Caves of Qud 等游戏显示 DX12 而非实际运行时 API 的问题。
- 名称中带有商标符号（®、™）的游戏现在正确匹配 RHI 数据库条目 — Borderlands® 4 等之前无法被找到。
- 导入中含有 DX11 兼容垫片的虚幻旧版（UE1/2/3）游戏现在正确显示 DX9 作为其主要 API。
- PCGamingWiki API 检测现在除 DX11/DX12 外还读取 DX9、DX10、Vulkan 和 OpenGL。PE 扫描无结果的游戏现在以 PCGW 数据为准。

### 错误修复

**崩溃与冻结**
- 修复了影响安装、Nexus 登录、拖放、Update All、Luma 安装和使用 `--launch` 启动应用的间歇性 UI 冻结。
- 修复了长会话期间累积的缓慢内存和连接泄漏。
- 修复了每次打开游戏卡时事件处理器累积的泄漏。

**Luma**
- 修复了 Luma 卸载在 ReShade 仍安装时删除着色器文件夹的问题 — 着色器现在被保留并重新部署。
- 修复了 Luma 卸载留下 `reshade-shaders-original` 文件夹的问题。
- 修复了 Luma 卸载使 `reshade.ini` 保持 Luma 配置状态的问题 — 现在部署保留你快捷键和设置的新副本。
- 修复了 Luma 卸载留下 `nvngx_dlss.dll` 的问题。
- 修复了 Nexus 上的 Luma Mod 反复显示"有可用更新"的问题 — 这是由比较页面编辑时间戳而非实际文件发布引起的误报。

**Game Pass**
- 修复了 Game Pass 游戏更新后 ReShade 丢失的问题 — Windows 更新时会用新的版本化路径替换安装文件夹。RHI 现在检测到这一点并在下次启动时自动重装 ReShade。

**其他**
- 修复了 PCGamingWiki 链接从不出现在游戏卡上的问题。
- 修复了渲染缩放输入框按 Enter 后保持键盘焦点的问题。
- 修复了同时扫描多个游戏时偶尔出现错误 DLSS 扫描计数的问题。
- 修复了文件被其他进程锁定时文件清理静默中止的问题。
- 窗口位置和大小现在在你完成移动或调整窗口大小时保存，而不仅仅在干净关闭时 — 因此即使 RHI 被强制关闭或被更新重启，你的布局也会保留。
- 状态栏中的版本号现在可点击并检查应用更新。

### 清单更新

- 将 "Banishers: Ghosts of New Eden - The Wanderer Set DLC" 添加到黑名单 — 之前被错误检测为游戏。
- 从仅外链列表中移除了艾尔登法环和艾尔登法环：黑夜君临 — 两者的 Mod 现在在 RHI 中都有直接下载链接。
- 为叛乱：沙漠风暴添加了引擎提示。
- 为 Outlast 和 Outlast 2 添加了 DX9 API 覆盖 — 两者都因虚幻旧版 PE 导入垫片显示为 DX11。
- 更新了 MGS4 和和平行者（大师合集）的引擎提示为 KojiPro Engine。

### 诊断日志

此构建包含增强的会话日志，以帮助追查间歇性发生的 UI 冻结。每个主要 UI 渲染操作都会将计时和状态记录到会话日志。如果应用冻结，请关闭它并分享 `%LocalAppData%\RHI\Logs\` 中的日志 — 这将有助于缩小原因范围。

## v2.7.2

### 新增

- **RHI 数据库现在是默认 Mod 数据源** — RHI 现在使用自己的社区数据库而不是抓取 RenoDX wiki。该数据库更易于维护、更可靠，并让 RHI 完全控制数据，而不依赖第三方基础设施。还有一个安全方面的好处：RenoDX wiki 是公开可编辑的，随着 RenoDX 及 RHI 用户群的增长，确实存在有人上传伪装成合法 HDR Mod 的恶意文件的风险 — RHI 将无法阻止其交付给用户。RHI 数据库在任何更改上线前都受到控制和审查。特别感谢 Scrungus 将所有 wiki Mod 和详细信息迁移到新数据库的工作。如有需要，你仍可以在设置中切换回 RenoDX Wiki 抓取器。

### 错误修复

- 修复了在游戏之间导航时 UI 冻结的问题 — RHI 之前在每次选择卡片时都在 UI 线程上进行文件系统 I/O（AppData 按钮的目录扫描）。现在在启动时后台预计算并按游戏缓存。
- 修复了点击 Luma（以及其他潜在组件）上的安装后 UI 永久无响应的问题 — 着色器包设置读/写操作使用了同步锁，当后台下载任务同时持有该锁时可能阻塞 UI 线程。
- 修复了 DXVK 卸载在随后的 ReShade 模式切换期间阻塞 UI 线程的问题。
- 修复了着色器包下载活动期间打开着色器包选择器时 UI 短暂冻结的问题。
- 修复了设置页驱动组合框更改（着色器缓存、G-Sync、FPS 限制、ReBAR、VSync、电源模式等）导致短暂 UI 卡顿的问题 — 所有 NVAPI 配置写入现在分派到后台线程。
- 修复了游戏覆盖面板中按游戏的 NVIDIA 驱动设置组合框（VSync、低延迟、Smooth Motion、电源模式、G-Sync、ReBAR）每次更改导致短暂 UI 卡顿的问题。
- 修复了 NVIDIA Profile 区块中的 DLSS 预设组合框、渲染缩放和驱动覆盖开关每次更改导致短暂 UI 卡顿的问题。
- 修复了 RTX HDR 开关和配置 RTX HDR 应用按钮在 NVAPI 写入时阻塞 UI 线程的问题。
- 修复了 MFG 对话框（FG 模式、生成系数、目标 FPS）在 NVAPI 写入时阻塞 UI 线程的问题。
- 修复了拖放 `.addon` 文件（某些 Luma Mod 使用此扩展名）时显示 "No Addon Found" 的问题 — `.addon` 文件现在与 `.addon64` 和 `.addon32` 一起被接受。Luma 命名的插件文件现在正确通过 Luma 安装流程（含 ReShade 部署、dgVoodoo2、着色器等）而不是 RenoDX 插件流程。安装选择器现在显示 "🌙 Install Luma Addon" 并使用模糊文件名匹配预选正确的游戏。
- 修复了无主之地 2 和无主之地：前传上 ReShade 安装为 `dxgi.dll` 的问题 — 这些是应使用 `d3d9.dll` 的 DX9 游戏。`dxgi.dll` 覆盖原本用于 Luma+dgVoodoo 安装，但被错误地应用到了普通 ReShade 安装。
- 修复了 DX9+dgVoodoo 游戏上卸载 Luma 后 ReShade 保持为 `dxgi.dll` 的问题 — 现在会以正确的 `d3d9.dll` 文件名自动重装。
- 修复了安装 DLSS5 Feeder 时 DX9 游戏不部署 dgVoodoo2 的问题 — 它被错误地限制在 Luma dgVoodoo 清单列表中，而该列表只覆盖 Luma 专属游戏。dgVoodoo2 现在为所有 DX9 Feeder 安装部署。
- 修复了 DLSS5 Feeder 在 DX9 游戏上将 ReShade 安装为 `d3d9.dll` 的问题 — 这些游戏上 dgVoodoo2 占用 `d3d9.dll`，因此 ReShade 必须是 `dxgi.dll`。现有的错误安装会在重新安装时自动纠正。
- 修复了后台扫描在 DX9+dgVoodoo Feeder 游戏上将 ReShade 从 `dxgi.dll` 重命名回 `ReShade32.dll` 的问题 — 协调逻辑现在正确跳过 dgVoodoo2 处于活动状态的游戏。
- 修复了自动更新在 DX9+dgVoodoo Feeder 游戏上将 ReShade 安装为 `d3d9.dll` 的问题 — 与上述相同的修复应用于 Update All ReShade 路径。

### 清单更新

- 将 UltraShade（前身为 Ultra+ 的 Ultra ReShade）更新为新的仓库 URL 和显示名称。

## v2.7.1

### 新增

- **导出游戏数据** — 设置中复制日志旁的新按钮。收集你的游戏库数据（图形 API、exe 路径、引擎、商店 ID）并将 zip 复制到剪贴板。粘贴到 Discord 与社区分享 — 提交有助于 RHI 为更多游戏正确检测 API 和安装路径。
- **GitHub API 令牌支持** — 如果你遇到限流（OptiScaler NR 预下载不可用、版本检查未完成），创建 `%LocalAppData%\RHI\github_api.txt` 并单行粘贴 GitHub 个人访问令牌。无需权限范围 — 用于公开仓库的免费令牌在 github.com/settings/tokens 生成大约需要 30 秒。将限额从每小时 60 次提升到 5000 次 API 调用。
- **Luma 的 Nexus Mods 支持** — 托管在 Nexus Mods 上的 Luma Mod（如质量效应、荣誉勋章：空降神兵、无主之地 2）现在显示"在 Nexus Mods 上获取"按钮，直接跳转到下载页面。同时具有 GitHub 和 Nexus 链接的 Mod（如 Prey、生化奇兵：重制版）现在显示两个选项。完整的一键 Nexus 安装已接线，正等待 Nexus API 批准。
- **dgVoodoo2 自动检测** — RHI 现在通过读取 Luma wiki 的特别说明列，自动检测 Luma Mod 何时需要 dgVoodoo2（针对质量效应、无主之地 2、荣誉勋章：空降神兵等 DX9 游戏），包括该 Mod 推荐的具体 dgVoodoo2 版本。不再依赖硬编码列表。

### 变更

- **PCGamingWiki API 检测** — RHI 现在后台使用 PCGamingWiki 验证无法直接扫描的游戏（Xbox/Game Pass 游戏、拒绝访问的路径）的 DirectX 版本。光环：无限、Resonance 等之前不显示 API 徽章的游戏现在正确显示 DX12。
- **PCGamingWiki 配置路径检测** — RHI 现在从 PCGamingWiki 抓取配置文件位置，并用它将 Engine.ini 放到正确的文件夹。修复了尚未启动过的游戏的 UE-Extended 安装 — 配置文件夹尚不存在，RHI 之前只能猜测项目名称。
- **神经渲染：Feeder 和 Bridge 现在支持插件版本固定** — 插件版本下拉菜单现在对所有四种 NR 方法都激活。
- **OptiScaler 齿轮的 Upscaler API 默认为游戏检测到的 API** — DX12 游戏打开时默认为 DX12，而不是始终默认 DX11。
- **更可靠的 OptiScaler 和神经渲染安装记录** — RHI 现在在每次 OptiScaler 和神经渲染安装时向游戏文件夹写入 `rhi_install.txt` 文件，记录部署的确切版本、变体和文件列表。下次启动时用它显示正确版本（修复 nightly 版本在重启后恢复为稳定版本号的问题），卸载时用它确切知道要清理哪些文件，无论预下载文件夹此后是否已更新。

### 错误修复

- 修复了同时安装 NR 方法（DLSS5 Tool、ShortFuse、Feeder）时 OptiScaler 卸载留下 DLSS 文件的问题 — RHI 现在跟踪哪个组件拥有每个共享文件，只有部署它的最后一个组件被卸载时才移除文件。
- 修复了卸载 DLSS5 Tool 后 `nvngx_dlss*.dll` 文件留在游戏根目录（如 Mortal Shell II）的问题，这些游戏的实际 DLSS 位于插件子文件夹中。
- 修复了 OptiScaler DLL 命名覆盖未正确跟踪重命名的 DLL 导致卸载留下文件的问题。
- 修复了通过 Extras 区移除 MFG Ada Unlock 后每次刷新都重装的问题。
- 修复了游戏的安装文件夹不再存在时批量 DLSS/Streamline 部署卡住的问题。
- 修复了 DLSS5 DX11 Bridge 和 MFG Ada Unlock 每次启动都重新下载的问题。
- 修复了刷新期间着色器包文件夹偶尔被重命名为 `reshade-shaders-original` 的问题。
- 修复了移除神经渲染后 `renodx-dlss5.addon64` 重新出现在插件选择器中的问题。
- 修复了标准刷新后 RHI 数据库（rhi-repo）更改不生效的问题 — 此前需要完整重启。
- 修复了刷新后当前选中游戏的详情面板不刷新的问题。
- 修复了 UE-Extended 游戏的 rhi-repo 数据库备注不出现在 RenoDX 信息对话框中的问题。
- 修复了在 ReShade 截图快捷键框中按退格键被记录为快捷键的问题 — 退格键现在清除快捷方式（与 ReShade 自身行为一致）。感谢 @tzachbon 的贡献。
- 修复了截图路径为空时"截图与快捷键"中的"应用到所有游戏"无反应的问题 — 快捷键和效果列表样式现在无论如何都会应用。感谢 @tzachbon 的贡献。
- 修复了 OptiScaler 下载期间 UI 冻结几秒钟的问题 — 进度和状态消息更新不再触发完整的详情面板重建。感谢 @Nypheena 的贡献。
- 修复了干净 ReShade 安装时创建空 `reshade-shaders-original` 文件夹的问题 — 在没有先前 `reshade-shaders` 的新游戏文件夹上，RHI 意外地创建该文件夹并立即将其重命名为备份。卸载时这个空文件夹会被恢复为 `reshade-shaders`，只留下管理标记而没有着色器。
- 修复了通过拖放或下载监视器安装 Luma 时缺少多个安装后步骤的问题 — ReShade、DLSS、dgVoodoo2、Engine.ini 键和启动参数现在无论 Luma 压缩包如何安装都被正确应用。
- 修复了同时安装 dgVoodoo2 时 ReShade 在 DX9 游戏上被部署为 `d3d9.dll` 的问题 — dgVoodoo2 需要 `d3d9.dll` 来拦截游戏的 DX9 调用，因此 ReShade 现在正确安装为 `dxgi.dll`，挂钩 dgVoodoo2 的 DX11 输出。

### 清单更新

- 修复了 **Solasta 2** 的 Engine.ini 写入错误 AppData 文件夹的问题（使用了 `Brimstone` exe 子文件夹名称而不是游戏名称）。
- 将 **ReShade Screenshot Discord Fix** 添加到插件选择器 — 从 HDR 截图中剥离 cICP 色彩块，使 Discord 正确预览而不是显示褪色的颜色。
- 天国：拯救 2 添加到 64 位覆盖列表。
- 将 **Ultra ReShade by Ultra+** 添加到着色器选择器 — 单着色器的"穷人版 DLSS5"效果，包含泛光、对比度、霾/去霾和饱和度。
- Five Hearts Under One Roof 添加到 64 位覆盖列表。
- 质量效应（2007）安装路径纠正为 `Binaries` 子文件夹。
- 007 First Light 安装路径纠正为 `Retail` 子文件夹。

---

## v2.7.0

### 新增
- **OptiScaler DLSS NR 变体** — OptiScaler 版本选择器中与 Stable 和 Nightly 并列的第三个选项。使用内置 DLSS 5 神经渲染的社区分支。具有与 Nightly 相同的功能（Streamline、FG 设置、预设等），外加齿轮中新的神经渲染设置区，无需触碰配置文件即可调整 NR 行为。在 Extras 区的 OptiScaler 齿轮中切换变体。
- **32 位 DX9 游戏上的 DLSS5 Feeder** — 神经渲染中的 DLSS5 Feeder 方法现在完整支持 32 位 DX9 游戏（无主之地 2、巫师 2 等等）。RHI 自动处理所有设置 — 仍然一键完成。
- **DX9 游戏的 Luma 支持** — 无主之地 2、无主之地：前传、巫师 2、荣誉勋章：空降神兵和征服者现在可以使用 Luma。RHI 在 Luma 安装的同时自动处理 DX9 兼容层。仍然一键完成。
- **神经渲染插件版本选择器** — 神经渲染区块现在有插件版本下拉菜单。你可以在安装前将游戏固定到特定版本的 DLSS5 Tool 或 DLSS Tool（ShortFuse）— 当较新版本在特定游戏上引起问题时很有用。默认为最新版并照常自动更新。神经渲染已安装时选择器变灰；先卸载再切换。
- **ReShade HDR Metadata** — 选择器中的新插件。在 ReShade 旁边将 HDR 元数据写入交换链，修复依赖它校准 HDR 管线的电视和显示器上褪色或色调映射错误的 HDR 输出。适用于运行 ReShade 的任何 DX11/DX12 游戏。无需配置。

### 错误修复

- 修复了 Lilium HDR Shaders（以及其他着色器包）不获取新版本的问题 — 即使有更新的版本可用，磁盘上已有该包时更新会被静默跳过。
- 修复了全新安装时 MFG Ada Unlock 安装按钮永久变灰的问题 — 现在点击安装时按需下载。
- 修复了有清单安装路径覆盖的游戏（如巫师 2）首次启动时显示错误安装路径和缺失 Mod 状态的问题。
- 修复了 ReBAR 容量上限写入和读取错误值的问题 — RHI 中设置的值现在在 NVPI 中正确显示，NVPI 中设置的值也被 RHI 正确读回。
- 修复了输出色彩设置在多显示器设置中应用到错误显示器的问题。

---

## v2.6.9

### 新增

- **RTX 40 MFG Unlock v1.3** — 更新为新独立格式。不再需要 ASI Loader — 它是单个 DLL，可以用游戏加载的任何代理名称（version.dll、dinput8.dll 等）部署。RHI 安装时显示名称选择器，并在首次启动时自动清理任何旧的基于 ASI 的安装。
- **20/30 FG 解锁** — MFG 解锁区的新条目。将 DLSS 帧生成带到 RTX 20 和 30 系显卡（仅限 D3D12 游戏，无需 ReShade 或 ASI Loader）。安装前在齿轮中选择你的显卡代次（RTX 30 或 RTX 20）— RHI 处理其余部分并保持自动更新。
- **分辨率与色彩控制** — 游戏启动时自动切换到目标分辨率，退出时恢复。还包括输出色彩设置，按显示器控制色彩深度和 HDR 动态范围，无需打开 NVIDIA 控制面板。
- **独立 DLSS Enabler** — Extras 区的新行。将 DLSS Enabler 作为代理 DLL 直接安装到任何游戏文件夹，独立于 OptiScaler。自动更新。
- **MFG Ada Unlock** — Extras 区的新行。在 RTX 40 系显卡上解锁 DLSS 多帧生成（3x/4x+）。需要 ReShade。与 RTX 40 MFG Unlock 互斥。

### 变更

- 在 NVIDIA Profile Overrides 区更改 DLSS 预设、渲染缩放或驱动覆盖不再闪烁或重建面板 — 值立即写入，组合框保持你设置的样式。
- Extras 区安装按钮（ASI Loader、RTX 40 MFG、DLSS Enabler）安装后现在显示与组件区相同的蓝色已安装样式。
- Extras 区现在用分隔符分组：顶部为 ASI Loader，MFG 解锁（RTX 40 MFG、MFG Ada Unlock、20/30 FG 解锁），以及其他（OptiScaler、DLSS Enabler）。
- 神经渲染：选择 ShortFuse 方法时在 NR Cost Scaler 开关下方添加了说明，解释 Cost Scaler 现在内置在插件中。

### 维护

- RHI 现在使用哨兵文件跟踪部署到游戏文件夹的每个 DLL。如果游戏在该位置已有文件，原始文件会被备份并在卸载时恢复。如果该位置没有文件，则写入 0 字节标记以便 RHI 知道可以干净清理。这防止游戏原始 DLL 在卸载后丢失，并修复孤立残留文件。覆盖 OptiScaler、DLSS 版本切换、RE Framework 和 Luma。
- 添加了内部 RenoDX 数据库服务，从 RHI 仓库获取 Mod 和 UE-Extended 配置数据。包含仅限开发者的"RenoDX 数据源"设置（仅 Wiki / 仅数据库 / 混合），用于在完全脱离 wiki 抓取之前的测试和验证。

### 错误修复

- 修复了 DLSS Fix 在 `reshade.ini` 中写入错误 Streamline 路径的问题 — 它指向了错误的文件，导致 DLSS Fix 无法正确挂钩 Streamline。
- 修复了在 RHI 已扫描游戏后添加 Streamline 文件（如来自游戏更新或同一会话中的其他组件安装）时 DLSS Fix 配置完全不写入的问题。
- 修复了全新安装 RHI 后 NR Cost Scaler 开关整个会话保持变灰的问题 — 它在启动时后台下载，但开关未更新以反映这一点，需要安装+卸载的变通方法来解除变灰。
- 修复了 ASI Loader 卸载时留下其 INI 文件的问题（如卸载 `winmm.dll` 现在也会移除 `winmm.ini`）。
- 修复了在游戏的文件夹不再存在于磁盘上时点击设置中的"将峰值尼特应用到所有"或"应用到所有游戏"时崩溃的问题。
- 修复了管理员模式在某些系统配置上不被检测的问题 — RHI 现在使用更可靠的检查，即使标准提权检查返回 false 也能正确处理拥有完整管理员权限的账户。
- 修复了安装后立即显示 ReShade 未安装的神经渲染区块的问题 — 之前必须离开并返回才能看到正确状态。
- 修复了 RE Framework 更新后在卡片上显示过期构建号的问题 — 版本现在在下次更新检查时正确同步。
- 修复了 NVIDIA Profile Overrides 每次启动都显示 Default/Off 直到手动刷新的问题 — 由配置文件读取在 NVAPI 完成初始化前静默失败的启动时序问题导致。

### 清单更新

- 鬼武者：剑的觉悟：添加了超宽修复链接。
- 荒野大镖客：救赎 2：匹配到 RenoDX wiki 条目（Vulkan Mod）。

---

## v2.6.8

### 错误修复

- 修复了 NVIDIA Profile Overrides 区不应用预设和渲染缩放更改的问题 — 过期的后台扫描回调在用户更改后覆盖了面板，丢弃了新值。
- 修复了刷新后驱动设置区（VSync、ReBAR、Smooth Motion 等）不出现的问题。
- 修复了 DLSS 预设和渲染缩放显示 Default/Off 而不是实际驱动值的问题 — 读取现在直接转到实时驱动状态而不是过期的内存缓存。

### 清单更新

- 添加了 DLSS SR、RR 和 FG 版本 310.9.1。
- 添加了 Streamline 2.14.1。

---

## v2.6.7

### 变更

- ReShade 卸载现在保留游戏文件夹中的 `reshade.log`。
- ReShade 配置文件现在作为 `ReShade.ini` 而不是 `reshade.ini` 部署到游戏文件夹。
- 神经渲染 DLL 选择简化为 310.8.2 — ShortFuse 的修改版，支持所有 RTX 显卡（20/30/40/50 系）。
- 为 DLSS SR、RR、FG 和 Streamline 版本下拉菜单添加了工具提示，说明选择版本会将其复制到游戏文件夹、Default 和 Custom 的作用，以及 NVIDIA Override 的工作原理。

### 错误修复

- 修复了 DLSS 和 Streamline 默认值中自定义渲染缩放的问题 — 选择"Custom"现在显示文本框，可以输入具体百分比（33–100%）。
- 修复了窗口保持活动（可移动、可最小化）但所有按钮和控件停止响应的间歇性 UI 冻结。这可能在后台对话框已显示时打开齿轮、安装对话框或其他弹出窗口时发生。影响 Luma 设置齿轮、ASI Loader 齿轮、ShortFuse 设置、RTX 40 MFG 齿轮和安装警告提示。
- 修复了"正在检查更新…"进度对话框打开期间发现应用更新时可能发生的冻结 — 更新对话框会静默阻塞最多 10 秒。
- 修复了 RHI 启动游戏或更改最近游戏设置后更新任务栏跳转列表时的短暂冻结 — 更新现在在后台运行。
- 修复了批量部署 DLSS 对话框快速完成后偶尔留下阻断所有输入的幽灵覆盖层的问题。
- 修复了更改 DLSS 版本、预设、渲染缩放或驱动覆盖时整个覆盖面板消失并重建自身的闪烁问题 — 现在只有 NVIDIA Profile 区刷新。
- 修复了同时更改 DLSS 版本和点击区块标题时可能发生的冻结。
- 修复了选择游戏或安装 OptiScaler 时 NVIDIA Profile Overrides 区短暂空白的问题 — 该区块现在使用缓存值立即显示，并在后台静默更新。
- 修复了更改 Vulkan ReShade 渠道（Stable/Nightly/Custom）时将文件复制到 `C:\ProgramData\ReShade\` 期间阻塞 UI 最多 10 秒的问题。
- 修复了在 Stable 和 Nightly 变体之间切换或应用预设后 OptiScaler 齿轮可能在 UI 线程外重建的问题 — 在某些时序条件下可能导致冻结。
- 修复了 OptiScaler 安装和卸载进度不显示在 Extras 区的问题 — 进度条和状态消息现在直接出现在 OptiScaler 行下方。*（感谢 Sapphire）*
- 修复了安装或移除 ASI Loader 和 RTX 40 MFG Unlock 重置 Extras 面板滚动位置的问题。
- 修复了拖放辅助窗口在任务栏和 Alt+Tab 切换器中显示为单独条目的问题。*（感谢 Owen）*
- 修复了快速点击浏览游戏时的间歇性 UI 冻结 — NVIDIA Profile 区的后台扫描现在以低优先级运行，因此不会阻塞指针输入，DLSS/驱动行以原子方式而不是逐个元素重建。

### 清单更新

- 侠盗猎车手 V 增强版：添加了关于 DirectStorage 不兼容导致"无法保存配置"错误的 ReShade 安装警告，附带 DirectStorageFix 链接和使用旧版本 ReShade 作为替代的说明。
- 幻异旅人（Satisfactory）：添加了帧生成设置说明 — 在 ReShade 信息按钮中显示启用 DLSS FG 所需的 Engine.ini 键。

---

## v2.6.6

### 错误修复

- 修复了 DLL 命名覆盖开关切换后无响应的问题 — 覆盖面板重建时现在清除任何进行中的拖动状态，并且即使文件操作抛出异常，开关也始终通过 `finally` 块重新启用。
- 修复了最近游戏 Off 设置不从系统托盘右键菜单隐藏游戏的问题 — 设置关闭时所有调用点现在传递空列表。
- 修复了最近游戏 Off 设置不清除任务栏跳转列表的问题 — 跳转列表现在使用 `ICustomDestinationList` 而不是 `SHAddToRecentDocs`，让 RHI 完全控制以在设置关闭时清除它。
- 修复了以不同 DLL 名称重装 ASI Loader 时留下旧 DLL 的问题 — 现在在以新名称部署之前清理之前的安装（并恢复任何 Hooked 备份）。
- 修复了两个 DLSS5 变体并发下载时 ShortFuse DLSS Tool 下载因"文件正在使用"而失败的问题 — 每个变体现在使用自己的临时文件名。
- 修复了 zip 内的插件文件与预期的确切文件名不匹配时 ShortFuse DLSS Tool zip 解压失败的问题 — 回退到 zip 中的任何 `.addon64` 条目。
- 修复了 OptiScaler 安装/卸载状态消息出现在组件区的问题 — 由于 OptiScaler 位于 Extras 区，已在那里抑制。

---

## v2.6.5

### 错误修复

- 修复了 OptiScaler 安装和卸载不刷新 Extras 区的问题 — 已安装/已卸载状态现在立即更新，无需手动刷新。
- 修复了 DLL 命名覆盖开关启用或禁用后需要刷新才能交互的问题 — 开关操作完成后覆盖面板现在立即重建。
- 修复了 RTX 40 MFG Unlock 安装时不写入 UAL ini 文件（`version.ini`、`dinput8.ini` 等）的问题 — 没有它，UAL 不知道要加载 `RTX40MFG.asi`，ReShade 会停止工作。RHI 现在自动将所需的 `[GlobalSets]` 键写入匹配的 UAL 代理 ini 文件。

---

## v2.6.4

### 变更

- DLSS 超分辨率、光线重建和帧生成版本组合框现在包含 **NVIDIA Override** 作为可选项。选择它会将驱动的"最新 DLL"标志写入游戏的 NVIDIA 配置，等同于在 Profile Inspector 中启用"DLSS — 启用 DLL 覆盖"。选择任何其他版本或点击恢复 DLSS/SL 会清除覆盖。NVIDIA Override 也可在配置默认值中作为默认值（通过快速应用应用），并可作为批量部署中的可选项。
- 添加了对按游戏自定义 Engine.ini 文件的支持，托管在 rhi-repo `engine-files/` 文件夹中并在清单中按游戏名称引用。设置后，RHI 只获取并合并该文件中的键而不是标准 HDR 键集 — 对于完整键集会导致问题的游戏，可以精确控制写入内容。

### 错误修复

- 修复了 NR Cost Scaler 和 RTX 40 MFG Unlock 在会话中不检测新版本的问题 — 检查更新和 4 小时定时器现在绕过 ETag 缓存并始终获取新鲜的发布数据。
- 修复了新版本预下载后 NR Cost Scaler 和 RTX 40 MFG Unlock 不自动部署更新文件到游戏文件夹的问题。

### 清单更新

- 黑神话：悟空 — UE-Extended 安装/更新时应用自定义 Engine.ini 文件（只写入 `r.HDR.EnableHDROutput=1` 而不是标准键集）。该游戏的 UE-Extended 齿轮中的标准 HDR 和 LUT 控件被禁用。

---

## v2.6.3

### 新增

- RTX 40 MFG Unlock 添加到 Extras 区 — 在支持 Streamline FG 的游戏上为 RTX 40 系显卡启用超过 2x（最高 6x）的 DLSS 多帧生成倍数。需要先安装 ASI Loader 和 ReShade。
- DLSS NR Cost Scaler 添加到神经渲染区 — nvngx_dlssnr.dll 的代理，以降低的分辨率（默认 75%）运行神经模型，在保留原生分辨率细节的同时显著节省 GPU。安装 NR 方法前打开开关即可一键部署。

### 变更

- OptiScaler 从组件区移至 Extras 区，与 ASI Loader 并列。
- RenoDX 齿轮兼容性设置：为提高可读性，格式标签去掉了 Upgrade_ 前缀（如 `R10G10B10A2_UNORM` 而不是 `Upgrade_R10G10B10A2_UNORM`）。
- RenoDX 齿轮对话框加宽，兼容性设置标签不再截断。
- 安装神经渲染方法现在自动从全局集合中移除冲突的全局插件（DLSS5 Tool、DLSS Tool（ShortFuse））并立即清理其文件。
- 对于有原生 DLSS 的 DX12 游戏，神经渲染自动选择现在默认为 ShortFuse（之前是 DLSS5 Tool）。
- RTX 40 MFG Unlock 和 MFG Ada Unlock 互斥 — 安装一个会阻止另一个。
- DLSS/Streamline 版本组合框现在将 `Default (x.x.x)` 显示为单独的顶部条目而不是在列表中标记某个版本 — 现在可以选择包括原始版本在内的任何版本而不触发恢复。
- Streamline 版本现在从 `sl.common.dll` 而不是 `sl.interposer.dll` 读取。
- "ReShade Addons" 在着色器/插件下拉菜单中更名为"全局插件"。
- 在按游戏着色器选择器中已处于 Select 状态时点击"选择"现在重新打开选择器（与插件选择器行为相同）。

### 错误修复

- 修复了 NR Cost Scaler 和 RTX 40 MFG Unlock 服务不遵守会话范围 GitHub API 限流标志的问题 — 它们现在通过共享的 ETag 缓存。
- 修复了离线或被限流时 RenoDX 更新检查 HEAD 请求使 UI 挂起最多 100 秒的问题 — 现在 10 秒后超时。
- 修复了在原生附带 `nvngx_dlssnr.dll` 的游戏（如赛博朋克 2077）上将按游戏插件切换为 Off 时 `renodx-dlss.addon64` 不被移除的问题 — 过期移除防护现在正确区分 RHI 放置的 NR DLL 和游戏原生的。
- 修复了神经渲染方法自动选择为有备份 NR DLL 但无活动安装的游戏推断 DLSS5 Tool 的问题。

### 已知限制

- DLSS5 Feeder：尚未实现 32 位游戏支持。

### 清单更新

- 潜龙谍影 4：爱国者之枪 大师合集版链接到 Luma wiki 条目。
- 潜龙谍影 4：爱国者之枪 — 添加安装警告：安装 Luma 前切换到 DX11。
- Eternal Strands — 添加 UE-Extended 兼容条目（跳过 HDR 键，仅 LUT）+ INI 覆盖。

---

## v2.6.2

### 变更

- 切换神经渲染方法现在自动移除前一个方法安装的任何组件，为安装新方法提供干净起点。
- ShortFuse ASI 自动配置（ReShade 重命名 + UAL 安装）现在为可选而非默认启用。
- 详情视图现在是默认布局。所有用户在此版本首次启动时切换一次。

### 错误修复

- 修复了从 DLSS5 Feeder 切换走时不移除其部署的着色器（DLSS5_Feed.fx、lumenite_Kernel.fx）的问题。

---

## v2.6.1

### 变更

- 对于有原生 DLSS 的 DX12 游戏，神经渲染自动选择现在默认为 ShortFuse（之前是 DLSS5 Tool）。
- ReShade 设置齿轮：叠加键和截图键字段现在并排显示。

### 错误修复

- 修复了快速点击浏览游戏列表时 UI 挂起的问题 — 所有同步 NVAPI 驱动配置读取（DLSS 预设、驱动覆盖、VSync/ReBAR/Smooth Motion）现在在 UI 线程外获取。
- 修复了快速更改 DLSS/Streamline 版本下拉菜单时 UI 挂起的问题。
- 修复了切换 DLL 命名覆盖开关导致 UI 挂起的问题。
- 修复了使用 D3D12 Agility SDK 的游戏（如鬼武者：剑的觉悟）被检测为 DX11 的问题。
- 修复了神经渲染方法为有备份 NR DLL 但无活动安装的游戏错误地默认为 DLSS5 Tool 的问题。
- 修复了缓存文件不可用时 DLSS5 Feeder 部署 0 字节 nvngx_dlss.dll 的问题。

### 清单更新

- 鬼武者：剑的觉悟和 PRAGMATA 强制为 DX12 检测。
- 机械战警：暴乱之城 — 未竟事业 Engine.ini 配置路径纠正。

---

## v2.6.0

### 新增

- 详情视图区块（组件、游戏覆盖、神经渲染、Nvidia Profile 覆盖、管理）现在可折叠。点击区块标题切换打开或关闭。折叠状态跨重启保留。
- 详情视图区块可以通过拖动每个区块标题左侧的 ≡ 手柄重新排序。顺序跨重启保留。
- 详情视图中的新 Extras 区。包含 Ultimate ASI Loader — 将 UAL 代理 DLL 安装到任何游戏文件夹以启用 .asi 插件加载。从支持的完整 DLL 名称列表中选择（按位数过滤，带推荐徽章和冲突警告）。自动保持最新。当所选 DLL 名称已被游戏文件占用时自动处理 Hooked 链。
- ShortFuse DLSS Tool 现在安装时自动为 FrameGen 配置 ReShade：将 ReShade 重命名为 Reshade64.asi，自动安装 ASI Loader（winmm → version → dinput8 优先级），并将 HookStreamline=1 和 HookDirectX=1 写入 reshade.ini。通过神经渲染安装按钮旁的 ⚙ 齿轮控制 — 默认启用，可按游戏关闭。

### 变更

- RenoDX 在详情视图组件列表中更名为 RenoDX HDR。
- 版本号现在显示在插件面板中 MFG Ada Unlock、DLSS5 Feeder 和 DX11 Bridge 旁边（与 DLSS5 Tool 相同）。
- ASI Loader 状态现在与 ReShade 和 DLSS 版本一起出现在 ShortFuse 神经渲染状态行中。

### 错误修复

- 修复了 ShortFuse DLSS Tool 插件（renodx-dlss.addon64）在通过神经渲染区安装的游戏上每次启动和刷新都被作为过期文件移除的问题。
- 修复了移除神经渲染方法后 Nvidia Profile Overrides 区显示过期 DLSS 版本的问题 — 现在立即刷新，无需手动刷新。

### 清单更新

- 博德之门 3 强制为 64 位检测。
- 霍格沃茨之遗链接到 Marat 的 UE-Extended 插件。

---

## v2.5.9

### 错误修复

- 修复了 DLSS5 Feeder 刷新清除 lumenite 着色器文件的问题 — `SyncGameFolder` 删除所有托管着色器后只重新部署 DLSS5_Feed.fx，丢失了 lumenite_Kernel.fx。通过 SetExcludedFiles 持久化包级排除修复，刷新现在正确地只部署所需的两个文件。
- 修复了按游戏插件选择器中缺失 MFG Ada Unlock、DLSS5 Feeder 和 DX11 Bridge 的问题。

---

## v2.5.8

### 变更

- 点击 UE-Extended 游戏上已安装的版本号现在打开 Marat 的 UE-Extended 插件提交历史。

### 错误修复

- 修复了 OptiPatcher 不为 NVIDIA 用户部署的问题 — 它被错误地限制为仅 AMD/Intel。
- 修复了切换到基于 API 的自动更新后 MFG Ada Unlock、DLSS5 Feeder 和 DX11 Bridge 从全局插件管理器中消失的问题。
- 修复了 MFG Ada Unlock、DLSS5 Feeder 和 DX11 Bridge 不自动更新的问题 — 这些插件使用动态发布文件名，因此 RHI 现在从 GitHub releases API 解析下载 URL 而不是硬编码 URL。
- 修复了插件更新检查在清单应用之前运行的问题，该问题导致清单驱动的插件在每次启动时被静默跳过。
- 修复了标准刷新不重新检查之前确认为"无 DLSS"的游戏的问题 — 新安装的 DLSS（如添加帧生成的游戏更新）现在在标准刷新时显示，而不需要完全刷新。，该问题导致清单驱动的插件在每次启动时被静默跳过。

---

## v2.5.7

### 错误修复

- 修复了 DLSS5 Feeder 对某些用户下载失败的问题 — zip 文件名随每次发布而变化，因此硬编码的 URL 在更新时失效。RHI 现在从 GitHub releases API 动态解析下载 URL，因此 Feeder 今后会正确自动更新。
- 修复了 DOF Fix 安装失败的问题 — releases API 默认只返回前 30 个结果，随着仓库增长把 DOF Fix 的发布挤出了页面。现在使用 per_page=100。

---

## v2.5.6

### 错误修复

- 修复了神经渲染区部署的 `renodx-dlss5.addon64` 在下次刷新时被移除的问题 — 插件清理通道将其视为过期，因为它不是通过标准插件系统部署的。

---

## v2.5.5


### 新增

- **神经渲染区块** — 游戏详情面板中的专用独立区块（位于游戏覆盖和 NVIDIA Profile 覆盖之间），用于安装 DLSS 5 神经渲染。无需插件选择器。方法组合框有四个选项：
  - **DLSS5 Tool** — 面向原生 DLSS 游戏。部署 `renodx-dlss5.addon64`，将 DLSS SR/RR/FG 升级到最新，并部署 `nvngx_dlssnr.dll`。
  - **DLSS5 Tool + DX11 Bridge** — 面向 DX11/Vulkan 原生 DLSS 游戏。同上外加 `dlss5-bridge.addon64`（始终下载最新版）。
  - **DLSS Tool（ShortFuse）** — 任何 64 位原生 DLSS 游戏的替代方案。通过哨兵模式部署完整的 DLSS SR/RR/FG/NR 栈和 Streamline。
  - **DLSS5 Feeder** — 无原生 DLSS 游戏的默认选项（DX11、DX12、Vulkan、OpenGL、32 位）。自动部署 Feeder 插件、作为神经消费者的 DLSS5 Tool、`nvngx_dlss.dll`、`nvngx_dlssnr.dll` 和所需的着色器（`DLSS5_Feed.fx` + LumeniteFX 运动矢量）。写入两项技术都以正确渲染顺序预启用的 `ReShadePreset.ini`。
  - 若 ReShade 尚未安装则自动安装。
  - NR DLL 版本选择器、带版本的按文件状态指示器、安装/重新安装/移除按钮、现有安装的自动方法检测，以及每个方法的带链接说明。

### 清单更新

- 为奥日与黑暗森林：终极版添加了警告，提示通用 Unity Mod 可能有视觉问题，且命名 Mod 已弃用。
- 为巫师 3：狂猎 完整版添加了安装路径覆盖（`bin\x64_dx12`）、引擎提示（REDengine）和图形 API 覆盖（DX12）。
- 修复了 Outlast 检测为 32 位并解析到错误路径的问题 — 现在强制 64 位，带有 `Binaries\Win64` 路径覆盖，引擎提示设为虚幻（旧版）。
- 修复了 DLSS5 DX11 Bridge 下载 URL — 旧仓库已删除；更新为 `NIGos/dlss5-bridge` 及正确文件名 `dlss5-bridge.addon64`。

---

## v2.5.4

### 变更

- 点击"检查更新"现在也会在检查完成后立即触发静默自动安装通道，因此发现的任何更新都会被安装，无需单独点击"全部更新"（启用自动更新时）。
- 设置中的"导出配置"/"导入配置"按钮更名为"备份配置"/"恢复配置"以示清晰。
- ReBAR 启用现在有三个选项：自动（默认）、关闭和开启 — 反映新的驱动设置（0x000BFA21）。此前只有关闭和开启。全局设置页和按游戏覆盖面板都已更新。

### 错误修复

- 修复了卸载 DLSS5 Tool 时 `nvngx_dlssnr.dll` 不从游戏文件夹移除的问题。RHI 现在使用哨兵文件跟踪是否由它放置了该 DLL，因此只移除它部署的内容。
- 修复了自动更新设置为否时重启后恢复为是的问题。
- 修复了一个 URL（如 32 位变体）返回 404 时插件下载完全中止的问题 — 其余 URL 现在独立继续。
- 修复了插件模式切换到全局再切回时按游戏插件选择丢失的问题。
- 修复了预下载文件缺失时预选插件启动时不重新下载的问题。

---

## v2.5.3

### 错误修复

- 修复了 v2.5.2 之后 `RenoDX DLSS5.addon64` 仍被部署到游戏文件夹的问题。`settings.json` 中存储的按游戏插件选择仍引用旧名称（`RenoDX DLSS5`）— 现在在加载时迁移为 `DLSS5 Tool`。这与 v2.5.2 中的全局插件列表和过期文件修复是分开的。

---

## v2.5.2

### 错误修复

- 修复了由于重命名为 DLSS5 Tool 后残留的过期文件，`RenoDX DLSS5.addon64` 在每次启动时被部署到游戏文件夹的问题。RHI 现在在启动时自动移除它，并从所有受影响的游戏文件夹中清理，包括仍引用旧名称的按游戏插件选择。
- 修复了 DLSS5 Tool 和 DLSS Tool（ShortFuse）在 32 位游戏上被部署为 `.addon32` 导致 ReShade 加载错误的问题。两个插件现在始终部署为 `.addon64`。

---

## v2.5.1

### 错误修复

- 修复了 DLSS5 Tool 插件被选中后不部署到游戏文件夹的问题。内部包名从 "RenoDX DLSS5" 更改为 "DLSS5 Tool" 未反映到所有部署路径。
- 修复了重命名前版本的过期 `RenoDX DLSS5.addon64` 文件在每次启动时被重新部署到游戏的问题。RHI 现在在启动时自动移除它。
- 修复了从 DLSS Tool（ShortFuse）切换走时不清理共同部署的 DLSS 和 Streamline 文件的问题。RHI 放置的文件现在在取消选择时被完全恢复或移除。
- 修复了 DLSS5 Tool 和 DLSS Tool（ShortFuse）之间的互斥问题 — 现在选择一个会使插件选择器中的另一个变灰。

---

## v2.5.0

### 新增

- 为着色器包选择器添加了搜索栏 — 按包名称或单个着色器文件名过滤。
- **DLSS Tool（ShortFuse）** — ShortFuse 的 DLSS5 插件现在作为 DLSS5 Tool 之外的第二个选项出现在插件选择器中。支持 DX12、DX11 和 DX9 及 HDR 缩放。安装时，RHI 自动下载最新的 DLSS SR、RR、FG、NR 和 Streamline 文件并部署到游戏文件夹。支持 RTX 20-50 系。仍在开发中 — 如遇问题请回退到 DLSS5 Tool。
- **更新了 nvngx_dlssnr.dll** 到 ShortFuse 的最新构建，现在支持 RTX 20、30、40 和 50 系显卡，在 RTX 50 系上与原始 NVIDIA 构建性能一致。

### 变更

- 将神经渲染列移动到 Nvidia Profile 区的最右侧，位于 Streamline 之后。
- RenoDX DLSS5 插件更名为 DLSS5 Tool。当前版本现在显示在插件选择器中名称的旁边。

---

## v2.4.9

### 新增

- **nvngx_dlssnr.dll 310.8.SF** — ShortFuse 修改的神经渲染 DLL，将支持扩展到 RTX 20、30、40 和 50 系显卡。现在是 RHI 部署的默认版本。在 Windows 资源管理器中显示为 `310.8.1`，在 RHI 中显示为 `310.8.SF`。

### 变更

- 神经渲染部署 DLL 按钮现在还会将 `nvngx_dlss.dll` 与 `nvngx_dlssnr.dll` 一起部署到游戏文件夹。任何现有的 `nvngx_dlss.dll` 首先备份为 `.original`。
- 在状态栏中补丁说明旁添加了 MOTD 按钮 — 随时点击即可重新阅读当前消息。

### 清单更新

- 将 JakobPCoder 的 Reshade Motion Estimation 添加到着色器包库 — 稠密的实时光流运动估计。

---

## v2.4.8

### 错误修复

- 修复了"如何使用"链接不出现在按游戏插件选择器中的问题。
- 修复了 `renodx-dlss5.addon64` 被双击或拖放时触发安装提示的问题。它由 RHI 内部管理，只应通过插件选择器安装或放入自定义插件文件夹。

### 清单更新

- 将 DLSS5 DX11 Bridge 和 DLSS5 Feeder 添加到插件选择器 — 两者都在 D3D11 游戏中启用 DLSS 5 神经渲染。需要额外的设置步骤；每个插件上的"如何使用"按钮链接到仓库说明。
- 将 DLSS5 Feeder 配套着色器添加到着色器包库。
- 修复了潜龙谍影 4（大师合集）显示为虚幻引擎的问题 — 现在正确显示 MGS4 引擎。

---

## v2.4.7

### 错误修复

- 修复了部署 `nvngx_dlssnr.dll` 后神经渲染列不显示其为已安装的问题。现在立即更新，无需刷新。
- 自定义 DLL 激活时，神经渲染列现在清楚地显示"Custom"。

---

## v2.4.6

### 错误修复

- 修复了新版本发布时 RenoDX DLSS5 不自动更新到游戏的问题。该插件现在直接从自己的预下载文件夹部署更新后的文件，不再在插件文件夹中创建冗余副本。

### 清单更新

- 将 aron7awol 的 CubeLUT3Ddith 添加到着色器包库 — 带抖动的 Cube 3D LUT 着色器，减少色带。

---

## v2.4.5

### 错误修复

- 修复了插件预下载文件夹被删除后 RenoDX DLSS5 不部署到游戏文件夹的问题。该插件现在直接从自己的预下载位置部署。

---

## v2.4.4

### 新增

- **RenoDX DLSS5 插件** — `renodx-dlss5.addon64` 现在是按游戏插件选择器中的一等插件，列在 RenoDX Upgrade 之上。通过插件组合框 → 选择按游戏启用。RHI 自动下载它，与其他组件一起静默保持更新，并将 `nvngx_dlssnr.dll` 部署到游戏文件夹（如果尚不存在）。仅适用于 50 系显卡。
