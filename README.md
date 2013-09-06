MaybeSupreme
=======
Elimianting not null check nesting since 2013
## Background: ##

After reading [this](http://www.codeproject.com/Articles/109026/Chained-null-checks-and-the-Maybe-monad) post I really starting to like this "pattern".

So this is a slightly different version of it which also works for value types. And also with a couple of additional features.

 This is purely to eliminate null checks. And provide a "fluent" api for chaining data manipulation. It's a cool thing which makes your code leaner with the trade off of lesser comprehension for imperative developers.

Check the comments in the source for instructions how it works...