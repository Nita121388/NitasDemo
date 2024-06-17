因为很喜欢用Emoji表情包，而我没有找到满足我的需求的控件（或者说就是有瘾，就是想自己开发一个），于是就做了这个EmojiPciker控件，以供参考。

## 效果
![Demo](https://github.com/Nita121388/NitasDemo/assets/41347078/12fc35bd-4022-4d59-8ace-6d8950e517f0)


## 文件介绍
  1. `Nita.ToolKit.BaseUI `中为一些我练习WPF时，所做的基本控件
  2. `Nita.ToolKit.Emoji` 中多为参考的[`emoji.wpf`](https://github.com/samhocevar/emoji.wpf)项目代码，我修改了其中以写小问题，修改了字体和支持中文搜索

      这里引用了很多Json文件，字体自行相关的dll文件等
  3. `Nita.ToolKit.EmojiUI`中引用 `Nita.ToolKit.BaseUI `和项目`Nita.ToolKit.Emoji`,提供了控件EmojiPicker控件
  4. `Nita.ToolKit.EmojiDemo`中引用 `Nita.ToolKit.EmojiUI `,



# 项目参考

#### 主要参考项目：

[samhocevar/emoji.wpf](https://github.com/samhocevar/emoji.wpf)

它实现了Emoji表情的渲染逻辑，也实现了EmojiPicker等其他Emoj文本控件。我想在此基础上做一些小扩展，也更换了字体

#### 字体：

[mozilla/twemoji-colr](https://github.com/mozilla/twemoji-colr)

#### Emoji名称翻译：

[emoji-json: 适配了 简体中文 的 emoji.json 数据源，与 unicode 联盟的数据源保持同步并版本对齐 (gitee.com)](https://gitee.com/angelofan/emoji-json)

#### 支持中文搜索Emoji：

[angelofan/emoji-json: 适配了 简体中文 的 emoji.json 数据源，与 unicode 联盟的数据源保持同步并版本对齐 (github.com)](https://github.com/angelofan/emoji-json)





#### 后言

只是一个WPF小白，代码多有疏漏，请多指教🍦。


  
