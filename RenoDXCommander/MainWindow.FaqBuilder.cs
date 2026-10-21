// MainWindow.FaqBuilder.cs — Builds the FAQ/Quick Start guide content dynamically.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;

namespace RenoDXCommander;

public sealed partial class MainWindow
{
    private bool _faqBuilt;

    /// <summary>
    /// Builds the FAQ panel content. Called once when FAQ is first opened.
    /// </summary>
    private void BuildFaqContent()
    {
        if (_faqBuilt) return;
        _faqBuilt = true;

        var panel = FaqContentPanel;
        panel.Children.Clear();

        // Welcome section
        panel.Children.Add(BuildFaqSection(
            null, "欢迎使用 RHI", "AccentTealBrush",
            "RHI 会自动检测您的游戏，让您可以在同一个窗口中安装 HDR 模组、着色器包、帧率限制器，并管理 NVIDIA 驱动设置。以下是快速上手说明：",
            null));

        // Step 1: Select a Game
        panel.Children.Add(BuildFaqStep(1,
            "选择游戏",
            "您的游戏列在左侧侧边栏中。点击任意游戏即可查看其详情与可用操作。使用筛选标签（全部游戏、已安装、虚幻引擎等）和搜索框可快速查找具体游戏。",
            "提示：双击游戏卡片可直接启动游戏。将游戏的 .exe 可执行文件拖放到 RHI 窗口中，可手动添加未被自动检测到的游戏。"));

        // Step 2: Install ReShade
        panel.Children.Add(BuildFaqStep(2,
            "安装 ReShade",
            "RenoDX HDR 模组需要 ReShade 才能运行。点击游戏详情面板上的“安装 ReShade”。RHI 会自动下载并安装具备完整插件支持的合适版本。",
            "可在游戏覆盖项中按游戏切换 ReShade 版本 — 可选 Stable（稳定版）、Nightly（每日构建版）、Legacy（旧版本）或自定义 ReShade DLL。\nVulkan 游戏：Vulkan 游戏（如《毁灭战士：永恒》）需要管理员权限。RHI 会在需要时提示进行权限提升。\n将 ReShade 预设文件 (.ini) 拖放到游戏卡片上即可自动安装。"));

        // Step 3a: RenoDX
        panel.Children.Add(BuildFaqSection(
            "3a", "RenoDX", "AccentTealBrush",
            "RenoDX 是 RHI 的主要 HDR 模组框架。每款游戏的 RenoDX 行会显示当前可用的方案：\n\n• 专属模组 — 来自 RenoDX Wiki、由社区制作的针对具体游戏的专属 HDR 模组。\n• UE-Extended — 适用于没有专属模组的虚幻引擎游戏。通过通用虚幻插件提供原生 HDR 输出。\n• Unity 插件 — 同样适用于 Unity 引擎游戏。为尚无专属模组的 Unity 游戏提供 HDR 输出。\n• RTX HDR — 如果没有可用模组，您可以通过 RenoDX 旁的 ⚙ 齿轮图标启用 NVIDIA 驱动级的 SDR 转 HDR 转换。",
            "RenoDX 旁的齿轮图标可打开高级设置：峰值亮度（尼特）、UE-Extended 开关以及 RTX HDR 配置。\nEngine.ini 设置（仅限虚幻引擎游戏）：切换写入游戏 Engine.ini 的 HDR 键值与 LUT 刷新频率，以确保精准的 HDR 渲染。\n对于不在 Wiki 上的游戏，可将来自 RenoDX Discord 的 .addon64 文件直接拖放到 RHI 中的游戏卡片上。"));

        // Step 3b: Luma
        panel.Children.Add(BuildFaqSection(
            "3b", "Luma", "AccentTealBrush",
            "Luma 是由 Pumbo (HDR Den) 开发的备用模组框架。根据游戏的不同，Luma 模组可能会添加 HDR、DLAA（深度学习抗锯齿）、游戏专属渲染修复或它们的组合 — 点击“信息”按钮可查看每个模组的具体特性。\n\n• 已完成模组 — 针对受支持游戏的专属 Luma 模组，显示在 Luma 行中。\n• 通用 Luma — 适用于所有 DX11 虚幻引擎游戏。RHI 会自动安装并应用 Luma Wiki 上列出的游戏专属 Engine.ini 调整或启动参数。\n\n您可以在同一款游戏上同时安装 RenoDX 和 Luma — 但无法保证它们在所有作品中都能完美共存。在您首次尝试同时安装两者时，RHI 会弹出提示。",
            "Luma 需要依赖 ReShade — 在安装 ReShade 之前它将呈灰色禁用状态。\nLuma 旁的 ⚙ 齿轮图标允许您为需要 TAA 的游戏切换 Engine.ini 设置。\n如果某款游戏需要特定启动参数（如 -dx11），RHI 会在安装时自动配置，并在卸载时自动移除。启动参数仅在通过 RHI 启动游戏时生效。\n点击 Luma 行上的“信息”按钮可查看游戏专属说明 — 已完成的模组通常包含模组新增功能的详细信息或所需的游戏内设置要求。"));

        // Step 4: Choose Shaders
        panel.Children.Add(BuildFaqStep(4,
            "选择着色器包（可选）",
            "点击工具栏中的“着色器 / 插件”按钮，然后选择“全局着色器”以挑选着色器包。默认已选中 Lilium 的 HDR 着色器包。这些设置将应用于所有已安装 ReShade 的游戏。\n\n展开任意着色器包即可单独挑选单个着色器 — 仅选中部分文件时包名旁边会显示破折号。使用右侧的配置方案面板可保存、加载、重命名和分享自定义着色器组合。还可将配置方案导出为 zip 压缩包以便在 Discord 中分享。",
            "提示：每款游戏的独立着色器可通过游戏详情卡片上的“着色器”按钮单独配置（需已安装 ReShade）。\n提示：使用“全部展开 / 全部折叠”可一次性浏览所有着色器包。“取消全选”将清空选择。“导出”会将选中的着色器打包为 zip 复制到剪贴板 — 可直接粘贴到 Discord 进行分享。"));

        // Step 5: DOF Fix
        panel.Children.Add(BuildFaqStep(5,
            "景深修复 (DOF Fix，推荐 UE5 游戏使用)",
            "DOF Fix 将虚幻引擎 5.7 的景深渲染修复反向移植到了运行于 UE 5.0–5.6 的游戏中。当游戏受支持时，它会显示在游戏详情面板的“推荐组件”部分。建议配合 RenoDX 一起安装以获得最佳效果。",
            "DOF Fix 仅在符合条件的 UE 5.0–5.6 游戏中显示 — 不会出现在 UE4 或 UE 5.7+ 游戏中。\n其安装和卸载操作与其他组件完全一致。"));

        // Step 6: Frame Limiters
        panel.Children.Add(BuildFaqStep(6,
            "帧率限制器（可选）",
            "ReLimiter 和 Display Commander 是用于为可变刷新率 (VRR) 显示器提供精准帧率限制的 ReShade 插件。您可以在游戏详情面板中安装它们。推荐使用 ReLimiter，因为它由 RHI 同一团队开发。您可以在“设置”中配置全局目标 FPS，也可以通过齿轮图标按游戏单独设置，或在游戏内直接调整。",
            "按刷新率预设的 VRR 帧率上限（在最大刷新率以下预留余量以确保平稳 VRR）：\n• 60Hz → 59 FPS\n• 120Hz → 116 FPS\n• 144Hz → 138 FPS\n• 165Hz → 157 FPS\n• 240Hz → 224 FPS\n• 360Hz → 324 FPS\n这些数值均已预置在 RHI 的 FPS 下拉菜单中。"));

        // Step 7: DLSS/Streamline
        panel.Children.Add(BuildFaqStep(7,
            "更新 DLSS / Streamline（可选）",
            "包含 DLSS 或 Streamline DLL 的游戏在详情面板中拥有独立区域并显示版本信息。点击即可更新到最新版本。建议使用最新版本以获得最佳性能与画质。RHI 会自动备份原始文件，方便您随时还原。",
            "当新的 DLSS 或 Streamline 版本发布时，它们会自动出现在 RHI 中。可在“设置”中设置默认 DLSS 预设。每款游戏的独立预设可在游戏详情面板的 DLSS 区域中进行更改。"));

        // OptiScaler
        panel.Children.Add(BuildFaqSpecialSection("⚙", "AccentAmberBrush",
            "OptiScaler（可选）",
            "OptiScaler 可将 DLSS/XeSS 替换为备选缩放器（FSR、XeSS、Intel Arc），或在任意显卡上添加/修复帧生成支持。当游戏支持 OptiScaler 时，可在游戏详情面板中安装它。\n\nOptiScaler 行旁边的 ⚙ 齿轮图标可打开游戏独立设置。对于 Nightly（每日构建）通道，设置包括：\n• Streamline/DLSS Enabler — 向游戏文件夹部署 Streamline 与 DLSS Enabler 以支持 DLSS 帧生成。\n• 帧生成 — 配置 FG 输入、FG 输出、FG Nvngx 覆盖以及 HUD 修复。\n• 附加设置 — DLSS SR/RR 预设、渲染比例与翻转配速。\n• 预设方案 — 保存并跨游戏应用已命名的设置预设。\n• Engine.ini 设置（虚幻引擎游戏）— 膨胀运动矢量、FSR 崩溃修复、FSR-FG 交换链、缩放器插件。",
            "可在齿轮菜单中按游戏在 Stable 和 Nightly 通道之间切换 — Nightly 通道增加了帧生成和更多设置。\nOptiScaler 和 ReShade 可以共存。如果同时安装两者时遇到游戏崩溃，请尝试在“游戏覆盖项”面板中使用“DLL 命名覆盖”将 ReShade 重命名为其他 DLL 名称。\n显卡类型与 DLSS 输入设置（仅限 AMD/Intel）在安装前于“设置 → OptiScaler 设置”中配置。\n齿轮菜单中的“部署 OptiScaler.ini”按钮会将您配置的 INI 模板重新部署到游戏文件夹中。"));

        // Settings Overview
        panel.Children.Add(BuildFaqInfoSection("全局设置",
            "点击工具栏中的“设置”以配置所有游戏的默认选项：",
            new[]
            {
                "ReLimiter FPS：默认目标帧率",
                "DLSS 预设：默认超分辨率预设",
                "NVIDIA 驱动设置：垂直同步、低延迟、电源管理模式",
                "峰值亮度 (Peak Nits)：显示器的 HDR 峰值亮度",
                "ReShade 快捷键：自定义控制面板与截图按键"
            }));

        // NVIDIA Driver Settings
        panel.Children.Add(BuildFaqInfoSection("NVIDIA 驱动设置",
            "RHI 可以管理按游戏设置的 NVIDIA 驱动配置文件。这些设置可直接在每款游戏的详情面板中进行配置：",
            new[]
            {
                "垂直同步 (VSync)：开、关或自适应（快速同步）",
                "低延迟模式：超高 (Ultra)、开或关",
                "平滑运动 (Smooth Motion)：多帧生成（仅限按游戏设置）",
                "ReBAR：Resizable BAR（需要管理员权限）"
            },
            "垂直同步、低延迟和电源管理模式的全局默认值在“设置”中配置。按游戏的独立覆盖设置可直接在各游戏的详情面板中调整。"));

        // Vulkan Games
        panel.Children.Add(BuildFaqSpecialSection("V", "AccentPurpleBrush",
            "Vulkan 游戏",
            "Vulkan 游戏（带有“Vulkan”徽章）使用安装到 C:\\ProgramData\\ReShade 的全局 ReShade 隐式层。这需要管理员权限。当您在 Vulkan 游戏上安装 ReShade 时，RHI 会提示进行权限提升。",
            "所有 Vulkan 游戏共享同一个全局 ReShade 安装。在某款 Vulkan 游戏上更新 ReShade 会同时更新所有 Vulkan 游戏。\n按游戏的 RenoDX 插件和着色器仍会单独安装到各自的游戏文件夹中。"));

        // Adding Games Manually
        panel.Children.Add(BuildFaqSpecialSection("+", "AccentAmberBrush",
            "手动添加游戏",
            "如果某款游戏未被自动检测到，可将其 .exe 可执行文件直接拖放到 RHI 窗口中。RHI 会将其添加到您的游戏库并检测其引擎类型。",
            "您也可以将来自 RenoDX Discord 的 .addon64 文件拖放到任意游戏卡片上，以安装尚未收录在 Wiki 中的模组。"));

        // Updating Everything
        panel.Children.Add(BuildFaqSpecialSection("↑", "AccentGreenBrush",
            "一键全部更新",
            "点击工具栏中的“全部更新”，即可一次性更新所有游戏中已安装的全部组件。包括 ReShade、RenoDX 模组、ReLimiter、Display Commander 等等。",
            "有可用更新的游戏在侧边栏中会显示状态圆点。您可以在“设置”中配置哪些组件参与“全部更新”。"));

        // Troubleshooting - Full Refresh
        panel.Children.Add(BuildFaqSpecialSection("↻", "AccentBlueBrush",
            "故障排查：全量重新扫描",
            "如果遇到游戏缺失、安装路径变更或手动增删了 DLSS/Streamline 文件，请使用“设置”中的“全量重新扫描”从头重新扫描您的整个游戏库。",
            "“全量重新扫描”会清空缓存的游戏列表并重新检测一切。当普通的“刷新”按钮无法识别变动时请使用此功能。"));

        // System Tray
        panel.Children.Add(BuildFaqSpecialSection("◰", "AccentPurpleBrush",
            "系统托盘运行",
            "RHI 可以最小化到系统托盘而不是直接退出。右键点击托盘图标可快速启动最近游玩的游戏，无需打开主窗口。",
            "在“设置”中启用“关闭时最小化到系统托盘”即可让 RHI 在后台保持运行。托盘图标提供了对最近游玩游戏的快捷启动入口。RHI 在运行时每 4 小时会自动检查一次更新，确保所有组件始终保持最新。"));

        // Need More Help
        panel.Children.Add(BuildFaqLinksSection());
    }


    private Border BuildFaqSection(string? badge, string title, string titleBrush, string description, string? tip)
    {
        var stack = new StackPanel { Spacing = 10 };

        var header = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };
        if (badge != null)
        {
            var badgeBorder = new Border
            {
                Background = (Brush)Application.Current.Resources[titleBrush],
                CornerRadius = new CornerRadius(12),
                Width = 24,
                Height = 24
            };
            badgeBorder.Child = new TextBlock
            {
                Text = badge,
                FontSize = 12,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 10, 30, 50)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            header.Children.Add(badgeBorder);
        }
        header.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 14,
            FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            Foreground = (Brush)Application.Current.Resources["TextPrimaryBrush"],
            VerticalAlignment = VerticalAlignment.Center
        });
        stack.Children.Add(header);

        stack.Children.Add(new TextBlock
        {
            Text = description,
            TextWrapping = TextWrapping.Wrap,
            FontSize = 12,
            Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
            LineHeight = 20
        });

        if (tip != null)
        {
            var tipBorder = new Border
            {
                Background = (Brush)Application.Current.Resources["SurfaceToolbarBrush"],
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12, 10, 12, 10),
                BorderBrush = (Brush)Application.Current.Resources["BorderSubtleBrush"],
                BorderThickness = new Thickness(1)
            };
            tipBorder.Child = new TextBlock
            {
                Text = tip,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 11,
                Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
                LineHeight = 18
            };
            stack.Children.Add(tipBorder);
        }

        return new Border
        {
            Background = (Brush)Application.Current.Resources["SurfaceRaisedBrush"],
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(20, 16, 20, 16),
            BorderBrush = (Brush)Application.Current.Resources["BorderSubtleBrush"],
            BorderThickness = new Thickness(1),
            Child = stack
        };
    }


    private Border BuildFaqStep(int step, string title, string description, string? tip)
    {
        var stack = new StackPanel { Spacing = 8 };

        var header = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        var badgeBorder = new Border
        {
            Background = (Brush)Application.Current.Resources["AccentTealBrush"],
            CornerRadius = new CornerRadius(12),
            Width = 24,
            Height = 24
        };
        badgeBorder.Child = new TextBlock
        {
            Text = step.ToString(),
            FontSize = 12,
            FontWeight = Microsoft.UI.Text.FontWeights.Bold,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 10, 30, 50)),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        header.Children.Add(badgeBorder);
        header.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 14,
            FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            Foreground = (Brush)Application.Current.Resources["TextPrimaryBrush"],
            VerticalAlignment = VerticalAlignment.Center
        });
        stack.Children.Add(header);

        stack.Children.Add(new TextBlock
        {
            Text = description,
            TextWrapping = TextWrapping.Wrap,
            FontSize = 12,
            Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
            LineHeight = 20,
            Margin = new Thickness(32, 0, 0, 0)
        });

        if (tip != null)
        {
            var tipBorder = new Border
            {
                Background = (Brush)Application.Current.Resources["SurfaceToolbarBrush"],
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12, 10, 12, 10),
                Margin = new Thickness(32, 4, 0, 0),
                BorderBrush = (Brush)Application.Current.Resources["BorderSubtleBrush"],
                BorderThickness = new Thickness(1)
            };
            tipBorder.Child = new TextBlock
            {
                Text = tip,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 11,
                Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
                LineHeight = 18
            };
            stack.Children.Add(tipBorder);
        }

        return new Border
        {
            Background = (Brush)Application.Current.Resources["SurfaceRaisedBrush"],
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(20, 16, 20, 16),
            BorderBrush = (Brush)Application.Current.Resources["BorderSubtleBrush"],
            BorderThickness = new Thickness(1),
            Child = stack
        };
    }


    private Border BuildFaqInfoSection(string title, string description, string[] bullets, string? tip = null)
    {
        var stack = new StackPanel { Spacing = 8 };

        var header = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        var badgeBorder = new Border
        {
            Background = (Brush)Application.Current.Resources["AccentBlueBrush"],
            CornerRadius = new CornerRadius(12),
            Width = 24,
            Height = 24
        };
        badgeBorder.Child = new TextBlock
        {
            Text = "?",
            FontSize = 12,
            FontWeight = Microsoft.UI.Text.FontWeights.Bold,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 10, 30, 50)),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        header.Children.Add(badgeBorder);
        header.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 14,
            FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            Foreground = (Brush)Application.Current.Resources["TextPrimaryBrush"],
            VerticalAlignment = VerticalAlignment.Center
        });
        stack.Children.Add(header);

        stack.Children.Add(new TextBlock
        {
            Text = description,
            TextWrapping = TextWrapping.Wrap,
            FontSize = 12,
            Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
            LineHeight = 20,
            Margin = new Thickness(32, 0, 0, 0)
        });

        var bulletStack = new StackPanel { Spacing = 6, Margin = new Thickness(32, 4, 0, 0) };
        foreach (var bullet in bullets)
        {
            bulletStack.Children.Add(new TextBlock
            {
                Text = $"• {bullet}",
                TextWrapping = TextWrapping.Wrap,
                FontSize = 11,
                Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
                LineHeight = 18
            });
        }
        stack.Children.Add(bulletStack);

        if (tip != null)
        {
            var tipBorder = new Border
            {
                Background = (Brush)Application.Current.Resources["SurfaceToolbarBrush"],
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12, 10, 12, 10),
                Margin = new Thickness(32, 4, 0, 0),
                BorderBrush = (Brush)Application.Current.Resources["BorderSubtleBrush"],
                BorderThickness = new Thickness(1)
            };
            tipBorder.Child = new TextBlock
            {
                Text = tip,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 11,
                Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
                LineHeight = 18
            };
            stack.Children.Add(tipBorder);
        }

        return new Border
        {
            Background = (Brush)Application.Current.Resources["SurfaceRaisedBrush"],
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(20, 16, 20, 16),
            BorderBrush = (Brush)Application.Current.Resources["BorderSubtleBrush"],
            BorderThickness = new Thickness(1),
            Child = stack
        };
    }


    private Border BuildFaqSpecialSection(string badge, string badgeBrush, string title, string description, string? tip)
    {
        var stack = new StackPanel { Spacing = 8 };

        var header = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        var badgeBorder = new Border
        {
            Background = (Brush)Application.Current.Resources[badgeBrush],
            CornerRadius = new CornerRadius(12),
            Width = 24,
            Height = 24
        };
        var badgeFg = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 10, 30, 50));
        badgeBorder.Child = new TextBlock
        {
            Text = badge,
            FontSize = badge.Length > 1 ? 11 : 14,
            FontWeight = Microsoft.UI.Text.FontWeights.Bold,
            Foreground = badgeFg,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        header.Children.Add(badgeBorder);
        header.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 14,
            FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            Foreground = (Brush)Application.Current.Resources["TextPrimaryBrush"],
            VerticalAlignment = VerticalAlignment.Center
        });
        stack.Children.Add(header);

        stack.Children.Add(new TextBlock
        {
            Text = description,
            TextWrapping = TextWrapping.Wrap,
            FontSize = 12,
            Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
            LineHeight = 20,
            Margin = new Thickness(32, 0, 0, 0)
        });

        if (tip != null)
        {
            var tipBorder = new Border
            {
                Background = (Brush)Application.Current.Resources["SurfaceToolbarBrush"],
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12, 10, 12, 10),
                Margin = new Thickness(32, 4, 0, 0),
                BorderBrush = (Brush)Application.Current.Resources["BorderSubtleBrush"],
                BorderThickness = new Thickness(1)
            };
            tipBorder.Child = new TextBlock
            {
                Text = tip,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 11,
                Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
                LineHeight = 18
            };
            stack.Children.Add(tipBorder);
        }

        return new Border
        {
            Background = (Brush)Application.Current.Resources["SurfaceRaisedBrush"],
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(20, 16, 20, 16),
            BorderBrush = (Brush)Application.Current.Resources["BorderSubtleBrush"],
            BorderThickness = new Thickness(1),
            Child = stack
        };
    }


    private Border BuildFaqLinksSection()
    {
        var stack = new StackPanel { Spacing = 10 };

        stack.Children.Add(new TextBlock
        {
            Text = "需要更多帮助？",
            FontSize = 14,
            FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            Foreground = (Brush)Application.Current.Resources["AccentTealBrush"]
        });

        stack.Children.Add(new TextBlock
        {
            Text = "Discord 社区提供支持，可加入社区获取帮助、模组更新和讨论。",
            TextWrapping = TextWrapping.Wrap,
            FontSize = 12,
            Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"],
            LineHeight = 20
        });

        var linksStack = new StackPanel { Spacing = 6 };

        var discordLink = new HyperlinkButton
        {
            NavigateUri = new Uri("https://discord.gg/ultraplus"),
            Padding = new Thickness(0)
        };
        discordLink.Content = new TextBlock
        {
            Text = "加入 Ultra+ Discord（主社区）",
            Foreground = (Brush)Application.Current.Resources["AccentBlueBrush"],
            FontSize = 12
        };
        linksStack.Children.Add(discordLink);

        var renodxDiscordLink = new HyperlinkButton
        {
            NavigateUri = new Uri("https://discord.gg/renodx"),
            Padding = new Thickness(0)
        };
        renodxDiscordLink.Content = new TextBlock
        {
            Text = "RenoDX Discord（模组开发）",
            Foreground = (Brush)Application.Current.Resources["AccentBlueBrush"],
            FontSize = 12
        };
        linksStack.Children.Add(renodxDiscordLink);

        var wikiLink = new HyperlinkButton
        {
            NavigateUri = new Uri("https://github.com/clshortfuse/renodx/wiki/Mods"),
            Padding = new Thickness(0)
        };
        wikiLink.Content = new TextBlock
        {
            Text = "浏览 RenoDX 模组 Wiki",
            Foreground = (Brush)Application.Current.Resources["AccentBlueBrush"],
            FontSize = 12
        };
        linksStack.Children.Add(wikiLink);

        var githubLink = new HyperlinkButton
        {
            NavigateUri = new Uri("https://github.com/RankFTW/RHI"),
            Padding = new Thickness(0)
        };
        githubLink.Content = new TextBlock
        {
            Text = "RHI GitHub — 报告问题或请求功能",
            Foreground = (Brush)Application.Current.Resources["AccentBlueBrush"],
            FontSize = 12
        };
        linksStack.Children.Add(githubLink);

        stack.Children.Add(linksStack);

        return new Border
        {
            Background = (Brush)Application.Current.Resources["SurfaceRaisedBrush"],
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(20, 16, 20, 16),
            BorderBrush = (Brush)Application.Current.Resources["AccentTealBorderBrush"],
            BorderThickness = new Thickness(1),
            Child = stack
        };
    }
}
