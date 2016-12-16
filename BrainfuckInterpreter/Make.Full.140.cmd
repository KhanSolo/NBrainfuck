SET msbuildexe= "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

rem .NET parts
%msbuildexe% /target:Clean BrainfuckInterpreter.sln /property:Configuration=Release

%msbuildexe% BrainfuckInterpreter.sln /property:Configuration=Release

pause