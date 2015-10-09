http://developer.android.com/training/basics/firstapp/creating-project.html

Activity: 
- provide user with access to app. 
- APP usually have a main activity when user launch it
- another activity when user selects some content to view

app/src/main/res/layout/activity_my.xml:  
- The screen UI.  
- in content_my.xml, there is a TextView element. 
- And it will show in screen UI.  

app/src/main/java/com.mycompany.myfirstapp/MyActivity.java:  
- Start the activity and load layout file.  

app/src/main/AndroidManifest.xml: 
- describe the fundamental characteristics and components.  

app/build.gradle: 
- Gradle is compiler and builder.  
- Each module has a build.gradle. This one is for app module.  
- The whole project also has a build.gradle.  
- Contain defaultConfig settings.  

res/drawable<density>:  
- drawable objects like bitmaps put here.  
- density: medium-density (mdpi) and high-density (hdpi) screens.  

res/menu/:  
- app's menu items.  

res/values/:  
- Other XML files that contain a collection resources.  

http://developer.android.com/training/basics/firstapp/running-app.html

