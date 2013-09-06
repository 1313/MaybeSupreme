using System;

namespace MaybeSupreme
{
    /// <summary>
    /// Maybe monad.
    /// An example implementation stolen from the source below 
    /// but with a few additional features.
    /// Read about it here: http://www.codeproject.com/Articles/109026/Chained-null-checks-and-the-Maybe-monad
    /// </summary>
    public static class F
    {
        /// <summary>
        /// With, can also be read as IfNotNull, 
        /// Used when you want to guard against null values before performing an operation.
        /// 
        /// Example: 
        /// var thirdField = someInstance.With(i => i.instanceField)
        ///                              .With(f => f.anotherField)
        ///                              .With(a => a.thirdField);
        /// 
        /// The imperative way would be:
        /// string thirdField;
        /// if(instance != null)
        ///    if(instance.instanceField != null)
        ///        if(instance.instanceField.anotherField != null)
        ///              thirdField instance.instanceField.anotherField.thirdField;
        /// 
        /// Or with less nesting using && operator.
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <typeparam name="TResult">Function which acts on input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="evaluator">Evaluating function which will act on input.</param>
        /// <returns>If input is not null, return result from evaluation.</returns>
        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator)
        {
            return Equals(input, default(TInput)) ? default(TResult) : evaluator(input);
        }


        /// <summary>
        /// Similar to With(), but also takes Fallback value to return if input/input is null.
        /// Powerfull when used together with chained with statements, concider the following.
        /// 
        /// var thirdField = someInstance.With(i => i.instanceField)
        ///                              .With(f => f.anotherField)
        ///                              .With(a => a.thirdField);
        /// 
        /// if any of the fields are null, thirdField will end up being null.
        /// With Return we can simply add a fallback value at the end to be 100% sure that thirdField 
        /// never will be null.
        /// 
        /// Like this:
        /// var thirdField = someInstance.With(i => i.instanceField)
        ///                              .With(f => f.anotherField)
        ///                              .With(a => a.thirdField)
        ///                              .Return(a => a, "FallBack");
        /// 
        /// if any of instance, instanceField, anotherfield, or thirdField happens to be null
        /// thirdField will get the value "FallBack"                          
        /// 
        /// </summary>
        /// <remarks>See documentation for With.</remarks>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <typeparam name="TResult">Function which acts on input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="evaluator">Evaluating function which will act on input.</param>
        /// <param name="failureValue">Fallback value if input happens to be null.</param>
        /// <returns>Result of evaluator or fallback value.</returns>
        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, TResult failureValue)
        {
            return Equals(input, default(TInput)) ? failureValue : evaluator(input);
        }

        /// <summary>
        /// Returns the Identity function for a desired type.
        /// Used for readability.
        /// Insetad of doing
        ///     instance.Return(x=>x, new Instance());
        /// 
        /// You can do:
        ///     instance.Return(F.Identity<Instance>(), new Instance());
        /// 
        /// First option is shorter, but second might be easier to grasp if you are used to the functional paradigm.
        ///   
        /// </summary>
        /// <remarks>
        ///     See: https://en.wikipedia.org/wiki/Identity_element
        /// </remarks>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <returns></returns>
        public static Func<TInput, TInput> Identity<TInput>()
        {
            return x => x;
        }

        /// <summary>
        /// Returns a function that throws an exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw</typeparam>
        /// <param name="ex">Exception instance</param>
        /// <returns>Throw function</returns>
        public static Action Throw<TException>(TException ex) where TException : Exception
        {
            return () => { throw ex; };
        }

        /// <summary>
        /// Similar as Return(Evaluator, Fallback), but instead of a value being used as fallback, an exception is thrown.
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <typeparam name="TResult">Function which acts on input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="evaluator">Evaluating function which will act on input.</param>
        /// <param name="exception">Exception to throw if input is null</param>
        /// <returns>Result of evaluator </returns>
        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, Exception exception)
        {
            if (Equals(input, default(TInput)))
            {
                throw exception;
            }
            return evaluator(input);
        }

        /// <summary>
        /// Similar as Return(Evaluator, Fallback), but instead of a value being used as fallback, an exception is thrown.
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <typeparam name="TResult">Function which acts on input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="evaluator">Evaluating function which will act on input.</param>
        /// <param name="failureAction">Failure Action to execute if input is null</param>
        /// <param name="fallbackValue">Fallback to return if input is null</param>
        /// <returns>Result of evaluator</returns>
        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, Action failureAction, TResult fallbackValue)
        {
            if (Equals(input, default(TInput)))
            {
                failureAction();
                return fallbackValue;
            }
            return evaluator(input);
        }

        /// <summary>
        /// Similar as Return(Evaluator, Fallback), but instead of a value being used as fallback, an exception is thrown.
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <typeparam name="TResult">Function which acts on input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="evaluator">Evaluating function which will act on input.</param>
        /// <param name="failureAction">Failure Action to execute if input is null</param>
        /// <returns>Result of evaluator </returns>
        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, Action failureAction)
        {
            if (Equals(input, default(TInput)))
            {
                failureAction();
                return default(TResult);
            }
            return evaluator(input);
        }

        /// <summary>
        /// Similar to Return(Evaluator, Fallback), but will call evaluator with parameters.
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <typeparam name="TParam">Type of parameters</typeparam>
        /// <typeparam name="TResult">Function which acts on input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="evaluator">Evaluating function which will act on input.</param>
        /// <param name="parameters">Parameters to call evaluator with</param>
        /// <param name="failureValue">Fallback value if input happens to be null.</param>
        /// <returns>Result of evaluator or fallback value.</returns>
        public static TResult Return<TInput, TParam, TResult>(this TInput input, Func<TInput, TParam, TResult> evaluator,
                                                              TParam parameters, TResult failureValue)
        {
            var nothingInInput = default(TInput);
            var nothingInParam = default(TParam);
            if (Equals(input, nothingInInput) || parameters.Equals(nothingInParam)) return failureValue;
            return evaluator(input, parameters);
        }

        /// <summary>
        /// Adds conditional logic, Similar to With() but instead of checking for null, 
        /// let evaluator determin if value should be returned or not.
        /// 
        /// Example:
        /// var thirdField = someInstance.With(i => i.instanceField)
        ///                              .With(f => f.anotherField)
        ///                              .If(a => a.thirdField == "Value")
        ///                              .Return(a => a, "Either something was null or thirdField did not equal value");
        /// 
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="evaluator">Evaluating function which will act on input.</param>
        /// <returns>Result of evaluator or null.</returns>
        public static TInput If<TInput>(this TInput input, Func<TInput, bool> evaluator)
        {
            var nothing = default(TInput);
            if (Equals(input, nothing)) return nothing;
            return evaluator(input) ? input : nothing;
        }


        /// <summary>
        /// Adds conditional logic, Similar() to With but instead of checking for null, 
        /// let evaluator determin if value should be returned or not.
        /// 
        /// Example:
        /// var thirdField = someInstance.With(i => i.instanceField)
        ///                              .With(f => f.anotherField)
        ///                              .Unless(a => a.thirdField == "Value")
        ///                              .Return(a => a, "Either something was null or thirdField was equal to value");
        /// 
        /// </summary>
        /// <remarks>Negetion of If()</remarks>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="evaluator">Evaluating function which will act on input.</param>
        /// <returns>Result of evaluator or null.</returns>
        public static TInput Unless<TInput>(this TInput input, Func<TInput, bool> evaluator)
        {
            var nothing = default(TInput);
            if (Equals(input, nothing)) return nothing;
            return evaluator(input) ? nothing : input;
        }

        /// <summary>
        /// Executes an action if input is not null.
        /// 
        /// Example:
        ///     instance.Do(i => i.instanceField = "Value");
        /// 
        /// Imperative version:
        ///     if(instance != null)
        ///         instance.instanceField = "Value";
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="action">Evaluating action which will act on input.</param>
        /// <returns>input if not null.</returns>
        public static TInput Do<TInput>(this TInput input, Action<TInput> action)
        {
            var nothing = default(TInput);
            if (Equals(input, nothing)) return nothing;
            action(input);
            return input;
        }

        /// <summary>
        /// Similar as Do(Evaluator), but if value is not ok, an exception is thrown.
        /// 
        /// Example:
        ///     instance.Do(i => i.instanceField = "Value");
        /// 
        /// Imperative version:
        ///     if(instance != null)
        ///         instance.instanceField = "Value";
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="action">Evaluating action which will act on input.</param>
        /// <param name="failureAction">Failure Action to execute if input is null</param>
        /// <returns>input if not null.</returns>
        public static TInput Do<TInput>(this TInput input, Action<TInput> action, Action failureAction)
        {
            var nothing = default(TInput);
            if (Equals(input, nothing)) return nothing;
            action(input);
            return input;
        }

        /// <summary>
        /// Similar to Do(Action). But calls action with parameters.
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <param name="input">input instance on which to act on if it's not null.</param>
        /// <param name="action">Evaluating action which will act on input.</param>
        /// <param name="parameters">Parameters to call action with.</param>
        /// <returns>input if not null.</returns>
        public static TInput Do<TInput, TParam>(this TInput input, Action<TInput, TParam> action, TParam parameters)
        {
            var nothing = default(TInput);

            if (Equals(input, nothing)) return nothing;
            action(input, parameters);
            return input;
        }
    }
}