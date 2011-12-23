Welcome to EveCache.Net, 
the C# EVE Cache Reader and Decoder Library based on libevecache!


Compiling
=========

Both projects should build out of the box targeting .Net 3.0+

The code doesn't use any exotic libraries, so I assume it probably works on
mono. I don't use mono though, so you are largely on your own.

Trying out Dumper
=================


After compiling both dumper and EveCache.Net, the following commands
should work:

Dumper 4b51.cache
Dumper --market 4b51.cache
Dumper --structure 4b51.cache


Integrating
===========

EveCache.Net is a standard C# class library

Everything is in the "EveCache" namespace.

For examples of how to use the library in the absense of other
documents, refer to the dumper utility located in the Dumper project.


Support
=======

E-mail  : jason at blacksunsystems.net
Code    : http://github.com/jwatkins42/EveCache.Net

libevecache Support Info
========================
NOTE: EveCache.Net IS NOT officially associated with libevecache, and you
should NOT expect them to know anything about the C# code. However, EveCahe.net
is structured very similarly to libevecache, so they may be able to answer
some general questions.

E-mail  : atrus at stackworks.net
IRC     : #evecentral on irc.freenode.org or #eve-dev on irc.coldfont.net
Code    : http://gitorious.org/libevecache
Web     : http://dev.eve-central.com/libevecache/
