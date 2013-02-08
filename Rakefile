require 'rake/clean'

MDTOOL = "/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"

APPS = %w{
	FieldService/FieldService.Xamarin.sln
	PrebuiltAppTheme/PrebuiltAppTheme.sln
	EmployeeDirectory/EmployeeDirectory.Xamarin.sln
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
