# Audience participation: industrial-grade continuous delivery

## "Send me a pull request during this talk!"

### Audience participation repository for the [DDD Brisbane 2016](http://dddbrisbane.com/) conference.

Everyone talks about continuous delivery, and almost everyone talks about how it’s “nice in theory but wouldn’t work here because reasons.” This talk is about that talk, and why that talk is wrong. Continuous delivery works - and works well, even (especially) for large, complex systems - but requires an engineering discipline that many organisations seem to think is beyond them.

In this talk we’re not going to talk so much about the “why” but focus on the “how”. We’re going to unapologetically focus on the Git/TeamCity/Octopus dream team and how to achieve the following:

  1. Every branch gets built and deployed to a CI environment on every push and every night.
  1. When `master` or a hotfix branch is green, out the door it goes.
  1. Packages auto-update every night and ship to production.

This talk will include live audience participation via pull requests. There will be a live Git repository (fork it now at
https://github.com/uglybugger/DDDBrisbane2016) with TeamCity configured to build your pull requests, merge them to
master and deploy them to production.

**_There will be precisely zero PowerPoint._ We’re going to run the whole session 
from a web application that we’ll collaboratively modify during the talk.**
