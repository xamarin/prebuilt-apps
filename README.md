# Xamarin Prebuilt Apps

Visit the [pre-built apps page](http://xamarin.com/prebuilt/) on xamarin.com.

---------------

To download this repository and and its dependencies:

```shell
git clone --recursive git@github.com:xamarin/prebuilt-apps.git
```

To build all apps:

```shell
rake build
```

## Employee Directory

Prototypical contacts application that can talk LDAP.

Available for Android and Windows Phone.

http://xamarin.com/prebuilt/employeedirectory

This sample has been removed for iOS because the "UISearchDisplayController" API was depercated in iOS 8+ and replaced with "UISearchController".
There is also a [bug](https://forums.developer.apple.com/thread/118297) with IOS 13+ which causes the sample to crash on iOS 13+.
Please see [this sample](https://github.com/xamarin/ios-samples/tree/061e5fd5a05c0f2d26ae27ef156b1d92c0deba5f/ios8/TableSearch) for an example of working with "UISearchController".

## Field Service

A field service app containing a list of assignments.  Take photos, signatures, record hours, etc.

Available for iOS, Android and as a Windows Store app (Windows 8 and Windows RT).

http://xamarin.com/prebuilt/fieldservice


### Note on the included solutions and projects

There are two solutions for each application: **Xamarin.sln** and **VisualStudio.sln**.

* The **Xamarin** solution contains iOS and Android projects, and is primarily targetted at developers on OS X using Xamarin Studio. 

* The **VisualStudio** solution contains Android and Windows projects, and is primarily targetted at developers on Windows using Visual Studio (since it can handle the Windows project types).

Alternatively you can open any of the individual platform project files, or you can choose to add the existing iOS project to your Visual Studio solution.

* To get the WinRT version working you need to install the Bing Maps VS extension: http://visualstudiogallery.msdn.microsoft.com/ebc98390-5320-4088-a2b5-8f276e4530f9

* Both samples utilize [Shared Projects](http://developer.xamarin.com/guides/cross-platform/application_fundamentals/shared_projects/), make sure that you use Visual Studio 2013 Update 2 or higher.
