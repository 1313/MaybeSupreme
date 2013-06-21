MaybeSupreme - Elimianting not null checks since 2013
============
# Background: #
After reading [this](http://www.codeproject.com/Articles/109026/Chained-null-checks-and-the-Maybe-monad) post (which is fairly old) I'm in love.

So this is a slightly different version of it which also works for value types.

I don't know much about Category Theory or Monads so if you're looking for a that please stop reading. This is purely to eliminate null checks. And provide a "fluent" api for chaining data manipulation. It's a cool thing which makes your code leaner with the trade off of lesser comprehension for imperative developers.

Anyhow, here's how you use it.

# Usage: #