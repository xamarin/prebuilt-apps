# Xamarin Prebuilt Apps

To build all apps:

```shell
rake build
```

## Employee Directory

Prototypical contacts application that can talk LDAP.

Available for iOS, Android and Windows Phone.

http://xamarin.com/prebuilt/employeedirectory

## Field Service

A field service app containing a list of assignments.  Take photos, signatures, record hours, etc.

Available for iOS, Android and as a Windows Store app (Windows 8 and Windows RT).

http://xamarin.com/prebuilt/fieldservice


###Note on the included solutions and projects

There are two solutions for each application: **Xamarin.sln** and **VisualStudio.sln**.

* The **Xamarin** solution contains iOS and Android projects, and is primarily targetted at developers on OS X using Xamarin Studio. 

* The **VisualStudio** solution contains Android and Windows projects, and is primarily targetted at developers on Windows using Visual Studio (since it can handle the Windows project types).

Alternatively you can open any of the individual platform project files, or you can choose to add the existing iOS project to your Visual Studio solution.

* To get the WinRT version working you need to install the Bing Maps VS extension: http://visualstudiogallery.msdn.microsoft.com/ebc98390-5320-4088-a2b5-8f276e4530f9