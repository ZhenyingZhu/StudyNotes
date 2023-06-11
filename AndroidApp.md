# Android

## Create project

<http://developer.android.com/training/basics/firstapp/creating-project.html>

Activity

- provide user with access to app.
- APP usually have a main activity when user launch it
- another activity when user selects some content to view

app/src/main/res/layout/activity_my.xml

- The screen UI.
- in content_my.xml, there is a TextView element.
- And it will show in screen UI.  

app/src/main/java/com.mycompany.myfirstapp/MyActivity.java

- Start the activity and load layout file.  

app/src/main/AndroidManifest.xml

- describe the fundamental characteristics and components.

app/build.gradle

- Gradle is compiler and builder.
- Each module has a build.gradle. This one is for app module.
- The whole project also has a build.gradle.
- Contain defaultConfig settings.

`res/drawable<density>`

- drawable objects like bitmaps put here.
- density: medium-density (mdpi) and high-density (hdpi) screens.

res/menu/

- app's menu items.

res/values/

- Other XML files that contain a collection resources.

## Run the app

### Install ADB driver

<https://www.youtube.com/watch?v=vUSRmtexmUo>

Install ADB driver on Windows

- Android\sdk\SDK Manager.exe: download usb driver.
- Device Manager: install driver from Android\sdk\extras\google\usb_driver
- cmd: `cd Android\sdk\platform-tools\; adb devices` to see if the device shows up.

<https://blog.csdn.net/ctcwri/article/details/14643611>

<https://www.xda-developers.com/quickly-install-adb/>

Download Android SDK: <https://developer.android.com/studio>

- Has its own JDK: `Program Files\Android\Android Studio\jre`. Gradle use it to build. But if `JAVA_HOME` is set, use that.
- Android SDK installed to `D:\AndriodSDK`

To connect to the virtal machine: <https://www.ucloud.cn/yun/2691.html>

- `adb connect 127.0.0.1:62001` (mumu: 7555, itools: 54001, nox: 62001)
- The second nox: `adb connect 127.0.0.1:62025`, run `Get-NetTCPConnection` in powershell, search for process id of `NoxVMHandle.exe`

### Run

<http://developer.android.com/training/basics/firstapp/running-app.html>

On device

- Enable USB debugging, then click run.

On Emulator

- Install HAXM installer through SDK Manager. `sdk\extras\intel\Hardware_Accelerated_Execution_Manager\intelhaxm.exe` (Doesn't work)
- Create AVD: Tools > Android > AVD Manager.

After gradle build, an apk shows under app\build\outputs\apk

## Build UI

<http://developer.android.com/training/basics/firstapp/building-ui.html>

Goal

- create a layout in XML that includes a text field and a button.

View Object: UI widgets like button or text field.
ViewGroup Object: view container. Define how child view are laid out, grid or vertical list.
Layouts: subclass of ViewGroup.

content_my.xml has a `RelativeLayout`, contain a `TextView`.  

Comment block in xml is `<!-- -->`  

Change `RelativeLayout` to `LinearLayout`. This replace the whole screen.  
Add the android:orientation attribute and set it to "horizontal" or "vertical".  
Each child of a LinearLayout appears on the screen in the order in which it appears in the XML.  
`android:layout_width` and `android:layout_height`, are required for all views in order to specify their size.  
Set `width` and `height` to "match_parent" fill up the parent view, which is screen here.  

Add `EditText` View with attributes

- `android:id`: id for this view.
- `android:layout_width` and `android:layout_height` set to `wrap_content`.
- `android:hint`: refer to `edit_message`. It is not the previous `edit_message` because type is not same(id vs. string).

`@` means refer to resource object from XML, follow with `type/resource_name`.
`+` before resource ID means define for the first time. After compile resource id shows in `gen/R.java`.

```xml
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:tools="http://schemas.android.com/tools"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:orientation="horizontal" >
    <EditText android:id="@+id/edit_message"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:hint="@string/edit_message" />
</LinearLayout>
```

Add `edit_message` string

- Add to res\values\strings.xml.
- name tag is the resource name.
- All UI string should be put here.

Add a Button

- "wrap_content" let button only as large as button text.
- `android:text`: text label.
- If it is not referenced from the activity code, then not need to set ID.
- `android:layout_weight`: let text field fill the unused screen.
- Weight: specify the amount of remaining space. Weight of this element divide sum of weights means the ratio of this element on the screen.
- `android:layout_width`: set to `0dp` can reduce the caculation of CPU to improve performance.

## Button activity

<http://developer.android.com/training/basics/firstapp/starting-activity.html>

Add activity to Button

- To the `<Button>` element, add the `android:onClick` attribute "SendMessage".
- So "SendMessage" is a method in java/com.mycompany.myfirstapp/MyActivity.java in `MyActivity` class.
- This method should be public, have void return, and input a `View`.
- New an Intent in the method.
- Find the `edit_message` by `findViewById` and return an `EditText` object.
- Put message from `edit_message` to intent using a unique key, like projectname.MESSAGE.
- Call `startActivity(intent);`

Intent Object

- provide runtime binding between components like two activities.
- mostly used to start another activity.
- its constructor has two parameters: `Context` like an `Activity` and the binding `Class`.
- `import android.content.Intent;`
- An Intent can carry data types as key-value pairs called extras.

Create a new Activity

- All subclasses of Activity must implement the `onCreate()` method.
- `onCreate()` method must define the activity layout with the setContentView() method.
- New > Activity > Blank Activity
- Every Activity is invoked by an Intent.

(Doesn't work with the newest android studio)

## Frida Framework

Setup: <https://www.cnblogs.com/lxh2cwl/p/14842537.html>

Cannot find frida terms: <https://github.com/frida/frida/issues/1000>

- `pip install frida`
- `pip install frida-tools`

Start Frida server

- `adb push D:\Downloads\frida-server-15.1.14-android-x86 /data/local/tmp`
- `adb shell`
- Rename the file to `fs`, and `chmod 777 fs`
- Run it `./fs &`
- On windows, run `frida-ps -U`, if see the process list, it is running then
- forward port `adb forward tcp:27042 tcp:27042`

Retrieve EnMicroMsg.db

- `adb pull /data/data/com.tencent.mm/MicroMsg/<the hash that contains the file>/EnMicroMsg.db D:\Downloads\EnMicroMsg.db`

Run a js

- (Doesn't work)

Install sqlcipher

- <https://stackoverflow.com/questions/61718992/install-pysqlcipher3-windows>
- <https://stackoverflow.com/questions/55446420/issue-in-installing-pysqlcipher3>

Other ways

- <https://zhuanlan.zhihu.com/p/123942610>
- <https://github.com/ppwwyyxx/wechat-dump>
- <https://github.com/chg-hou/EnMicroMsg.db-Password-Cracker>

Python system env needs

- PATH: `C:\Program Files\Python39\Scripts\`, `C:\Program Files\Python39\`, `C:\Users\<username>\AppData\Roaming\Python\Python39\Scripts`
- PYTHONPATH: `C:\Program Files\Python39\`, `C:\Program Files\Python39\\Lib`

## ADB

Pull the files to desktop `adb pull /sdcard/Movies .`

## Change cell phone resolution

Show the current resolution: `adb shell dumpsys display`

- Look for "real 1080 x 2400, largest app 1080 x 2400,"

Change it: `adb shell wm size 1080x1920`
