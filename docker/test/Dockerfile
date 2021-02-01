FROM mono:6.12.0

RUN nuget install NUnit.Console -o /packages
RUN nuget install NUnit.Extension.VSProjectLoader -o /packages

RUN echo 'mono /packages/NUnit.ConsoleRunner*/tools/nunit3-console.exe "$@"' > /usr/local/bin/nunit
RUN chmod +x /usr/local/bin/nunit
