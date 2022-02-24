FROM mcr.microsoft.com/dotnet/runtime:5.0
COPY bin/Debug/net5.0/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "DeribitFutureSubscriber.dll"]