require 'rake/clean'

XAMARIN_STUDIO = "/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"

if File.exists? XAMARIN_STUDIO
	MDTOOL = XAMARIN_STUDIO
else
	MDTOOL = "/Applications/MonoDevelop.app/Contents/MacOS/mdtool"
end

APPS = %w{
	FieldService/FieldService.iOS.sln
	PrebuiltAppTheme/PrebuiltAppTheme.sln
	EmployeeDirectory/EmployeeDirectory.sln
}

task :default => :build

# Builds a .sln file with MonoDevelop
def mdbuild solution, opts = {}
	sh "'#{MDTOOL}' build --configuration:Release '#{solution}'", opts
end

desc "Builds all prebuilt apps"
task :build do
	APPS.each do |sln|
		mdbuild sln
	end
end
