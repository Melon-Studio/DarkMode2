# ![Logo](./doc/ok.png)

![Tag](https://img.shields.io/github/tag/Melon-Studio/DarkMode2.svg)[![img](https://camo.githubusercontent.com/4c5e9973d91f9ac30425d8cdef2fb574b50f64e21cdad202be047f3848021b0a/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f666f726b732f4d656c6f6e2d53747564696f2f4461726b4d6f6465323f7374796c653d666c61742d737175617265)](https://github.com/Melon-Studio/DarkMode2/blob/master) [![img](https://camo.githubusercontent.com/b76728bc1c74684ee31f0be49f10ff005cd400a1ddae507d304be940b2a51412/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f73746172732f4d656c6f6e2d53747564696f2f4461726b4d6f6465323f7374796c653d666c61742d737175617265)](https://github.com/Melon-Studio/DarkMode2/blob/master) [![img](https://camo.githubusercontent.com/560c4d1a2d4d97df23b5148747dc88de44f51fdcb25254bb34144a041d7aaa22/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f6973737565732f4d656c6f6e2d53747564696f2f4461726b4d6f6465323f7374796c653d666c61742d737175617265)](https://github.com/Melon-Studio/DarkMode2/blob/master) [![img](https://camo.githubusercontent.com/5977bd502d8bba7c7aa9f76c04b1fc95ec64986900044e0c4e07b19ba5b9696f/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f6c6963656e73652f4d656c6f6e2d53747564696f2f4461726b4d6f6465323f7374796c653d666c61742d737175617265)](https://github.com/Melon-Studio/DarkMode2/blob/master) [![img](https://camo.githubusercontent.com/e9fbca5d0b8195869f2368539ad6eb31d979abd866bb8e4fc3165b5fae627f9a/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f6c6173742d636f6d6d69742f4d656c6f6e2d53747564696f2f4461726b4d6f6465323f7374796c653d666c61742d737175617265)](https://camo.githubusercontent.com/e9fbca5d0b8195869f2368539ad6eb31d979abd866bb8e4fc3165b5fae627f9a/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f6c6173742d636f6d6d69742f4d656c6f6e2d53747564696f2f4461726b4d6f6465323f7374796c653d666c61742d737175617265)[![img](https://camo.githubusercontent.com/05e612beecc0f77dc26faecb1b367a4323d11713fbc50c9a0904cca36fd24de2/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f64697363757373696f6e732f4d656c6f6e2d53747564696f2f4461726b4d6f6465323f7374796c653d666c61742d737175617265)](https://camo.githubusercontent.com/05e612beecc0f77dc26faecb1b367a4323d11713fbc50c9a0904cca36fd24de2/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f64697363757373696f6e732f4d656c6f6e2d53747564696f2f4461726b4d6f6465323f7374796c653d666c61742d737175617265)

DarkMode2 是一个开源软件，用于自动切换 Windows 10/11 系统的颜色模式。它提供了多项主要功能，包括定时切换、日出日落切换、感光切换、以及基于系统自带壁纸和 Wallpaper Engine 壁纸的切换功能。该软件的目标是解决 Windows 操作系统不支持自动切换颜色模式的问题。此外，它还提供了众多额外的功能。晚上自动切换到深色模式有助于减少眼睛疲劳，防止过多的光线进入眼睛，本软件虽然不能完全解决眼睛疲劳问题，但可以缓解在夜间使用浅色模式带来的眼睛疲劳问题。

---

## 👁️功能特点

- **定时切换功能**：基于用户定义的时间表，自动切换系统的颜色模式。
- **日出日落切换功能**：根据用户所在地的日出和日落时间，自动调整颜色模式。
- **感光切换功能**：根据环境光线的变化，自动切换颜色模式。
- **基于系统自带壁纸的切换功能**：根据系统自带壁纸的变化，自动调整颜色模式。
- **基于 Wallpaper Engine 壁纸的切换功能**：根据 Wallpaper Engine 设置的壁纸的变化，自动调整颜色模式。
- 更多功能正在开发中，欢迎提交 issues 开发者将视情况添加到软件的实验室中。

---

## 📀运行环境

- **操作系统**：Windows 10 / 11
- **系统架构**：x64 / arm64
- **必要框架**：.NET Framework 4.7.2

如果系统没有框架，请[点此安装](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net472-web-installer)

***注意**：在使用Beta版本时出现闪退等问题，请清除注册表键值，Beta版本处于快速迭代开发状态，很多功能都在变动。*

*清除方式：*

1. *在 Windows 搜索中搜索`注册表编辑器`，打开后在地址栏输入`计算机\HKEY_CURRENT_USER\Software\DarkMode2`，将这个`DarkMode2`项删除。*
2. *在设置中心的设置中，点击最下方`重置用户配置`按钮；*

---

## 🖱️安装与使用

1. [点击此处或者右侧Release](https://github.com/Melon-Studio/DarkMode2/releases)下载最新版本：
2. 在本地文件夹中，双击运行 DarkMode_2.exe。
3. 根据需要进行配置，并保存设置。
4. DarkMode2 会在后台运行，并根据您的设置自动切换颜色模式。

---

## 🧷开源协议

本开源项目遵循国际 MIT 开源许可证，具体内容请详细阅读 [LICENSE](https://github.com/Melon-Studio/DarkMode2/blob/master/LICENSE.txt) 文件。你可以在个人或商业项目中使用 DarkMode2 的全部代码，但是你必须在你的引用项目中包含MIT开源许可证的副本。

本项目使用的第三方开源库：

| 名称        | 作者            | 版权                                                         | 许可       |
| ----------- | --------------- | ------------------------------------------------------------ | ---------- |
| NHotKey.Wpf | Thomas Levesque | Copyright © 2020 Thomas Levesque Licensed under the Apache-2.0 License. | Apache-2.0 |
| WPF-UI      | lepo.co         | Copyright © 2022 lepo.co Licensed under the MIT License.     | MIT        |
| Log4net     | Apache          | Copyright © 2022 Apache Licensed under the Apache-2.0 License. | Apache-2.0 |
| ...         | ...             | ...                                                          | ...        |

更多依赖请[点击此处](https://github.com/Melon-Studio/DarkMode2/network/dependencies)查看。

---

## 🥰鸣谢

DarkMode2 特别感谢 [Microsoft Visual Studio](https://visualstudio.microsoft.com/) 提供强大的开发工具和支持。

![IDE](./doc/IDE.svg)

---

## ⚡为爱发电

为了能够持续提供免费的软件服务，我们现在开通了赞助渠道。通过这个渠道，您可以选择自愿为我们的软件项目提供赞助，这种支持对我们来说非常重要。

作为开发者，我深知自己的成长和软件的发展离不开您的支持和鼓励。您的赞助将直接用于改进软件的质量、功能和用户体验，以及为我投入更多的时间和精力，开发新的特性和功能。

我们郑重承诺，我们的软件将永远免费，永远保持更新，无论是现在还是将来。我们始终相信，开源软件的力量在于共享和协作，通过免费提供软件，我们可以为更多的用户带来实用和高质量的产品。

感谢每一位愿意赞助我们的用户，无论您的赞助数额大小，您的支持都将激励我继续努力并改进软件的品质。您的赞助也将成为我们前进的动力和持续发展的保障。

赞助渠道：https://afdian.net/a/DarkMode2

## 🎉贡献

如果您有兴趣为 DarkMode2 做出贡献，可以按照以下步骤进行：

1. 将该仓库克隆到本地：

   ```
   git clone https://github.com/Melon-Studio/DarkMode2.git
   ```

2. 在本地进行修改。

3. 提交您的修改，并创建一个 Pull Request（PR）。

4. 我们将审查您的 PR 并在批准后合并您的贡献。

感谢所有为 DarkMode2 做出贡献的开发者！

<a href="https://github.com/Melon-Studio/DarkMode2/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=Melon-Studio/DarkMode2" />
</a>

---

## 📶趋势

以下是本项目的最近星标趋势图：

[![Star History Chart](https://api.star-history.com/svg?repos=Melon-Studio/DarkMode,Melon-Studio/DarkMode2&type=Date)](https://star-history.com/#Melon-Studio/DarkMode&Melon-Studio/DarkMode2&Date)

请注意，这只是一个示例图表，显示了项目的最近趋势，并不代表实际数据。

欢迎关注我们的项目并给予星标！非常感谢您的支持！
