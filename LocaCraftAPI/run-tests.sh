#!/bin/bash
mkdir -p LocaCraftAPI.Tests/TestResults
dotnet test LocaCraftAPI.Tests/LocaCraftAPI.Tests.csproj --verbosity normal 2>&1 | tee LocaCraftAPI.Tests/TestResults/test-results.log
