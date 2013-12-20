#I "tools/FAKE/tools"
#r "FakeLib.dll"

#load "tools/XamarinHelper.fsx"

#nowarn "20"

open System
open System.IO

open Fake
open Fake.FileUtils
open Fake.RestorePackageHelper

open Fake.XamarinHelper

Target "Restore" <| fun _ ->
    RestoreComponents "FieldService/FieldService.Xamarin.sln"
    RestoreComponents "EmployeeDirectory/EmployeeDirectory.Xamarin.sln"
    RestorePackages ()

Target "Build" <| fun _ ->
    BuildiOSPackage "FieldService/FieldService.iOS/FieldService.iOS.csproj"
    BuildiOSPackage "EmployeeDirectory/EmployeeDirectory.iOS/EmployeeDirectory.iOS.csproj"

    BuildAndroidPackage "FieldService/FieldService.Android/FieldService.Android.csproj"
    BuildAndroidPackage "EmployeeDirectory/EmployeeDirectory.Android/EmployeeDirectory.Android.csproj"

Target "Clean" <| fun _ ->
    CleanDirs [
        "FieldService/FieldService.iOS/bin"
        "FieldService/FieldService.iOS/obj"
        "EmployeeDirectory/EmployeeDirectory.iOS/bin"
        "EmployeeDirectory/EmployeeDirectory.iOS/obj"

        "FieldService/FieldService.Android/bin"
        "FieldService/FieldService.Android/obj"
        "EmployeeDirectory/EmployeeDirectory.Android/bin"
        "EmployeeDirectory/EmployeeDirectory.Android/obj"
    ]

"Clean"
    ==> "Restore"
    ==> "Build"

RunTargetOrDefault "Build"
