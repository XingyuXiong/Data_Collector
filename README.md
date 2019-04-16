# Data_Collector
该应用是一个为接口微软api CustomVoice服务创建的使用Visual Studio 2017开发的wpf应用程序，功能是输入一个文本，对文本的每一行进行录音并打包压缩上传，
从而调用api根据录音创建一种可识别的，独一无二的语音，称之为语音字体。

Import Data
用于读取要录音的文本内容，读取的是txt文件。

Export Data
用于压缩已经在用户文件夹下存好的音频文件，生成一个可选择路径的名为audio的压缩包。

Content
搜索框可以将搜索内容换为Content/ID/Done属性，在每一行的相关属性查找关键词。
	比如选择Done属性，搜索框中输入Yes，找到的便是已经录音完的文本行。
  
SelectAll
按钮用于全选所有行。

Delete
按钮用于删除选中的行的音频文件（如果已有），文本并不会删去。
每行由三个属性，ID是一个数字用于指代第几行，对应的音频文件也以1.wav等命名，
	Content是txt文件每一行的文本，
	Done是是否完成，显示为Yes/No
下方显示正在录音的文本和进度，如Progress：1/57

Record
按钮用于录音，并且支持多项录音。当选择多项文本并点击Record按钮，会从所选文本第一项开始顺序录音，
	Record会变成Stop，点击Stop即可完成第一项的录音，如果该项之前已录音频则会覆盖掉，
	继续点击Stop则会完成下一项的录音，直至所有选中项均已录完则会弹窗提示录音完毕。
	如果录完一部分不想继续则点击Terminate结束多项录音。
	录音未完成前是不能播放的。
  
Play
按钮用于播放已经录制完成的wav音频文件。选择多项或单项都会循环播放，点击Terminate按钮则会结束播放，点击Pause暂停播放。



工程文件为Data_Collector_v1.1\Data_Collector下的.sln文件，用visual studio打开，打包的项目文件都在右侧的解决方案资源管理器中。


Data_Collector_v1.1\Data_Collector\Collector目录以下各文件包含如下逻辑：
App.config:配置文件
MainWindow.xaml:使用微软构建应用用户界面的描述性语言xaml编辑项目应用前端样式
MainWindow.xaml.cs:使用C#语言完成该应用应包含的后端功能
TextFile1.txt:用作测试custom voice服务的输入文本，虽然是一段代码但当作文本使用


Data_Collector_v1.1\Data_Collector\Data_Collector\Debug目录以下两个文件都是Visual Studio为wpf应用打包生成的安装程序文件，各文件包含如下逻辑：
Data_Collector.msi为微软windows installer开发的通用安装包，具有安装、修改、卸载程序的功能
setup.exe为通用安装包，同时可以检查用于安装的环境
