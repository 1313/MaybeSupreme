using System;

namespace MaybeSupreme
{
    public static class Extensions
    {
        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator)
        {
            return Equals(input, default(TInput)) ? default(TResult) : evaluator(input);
        }

        public static Func<TInput, TInput> Identity<TInput>()
        {
            return x => x;
        }

        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator,
                                                      TResult failureValue)
        {
            return Equals(input, default(TInput)) ? failureValue : evaluator(input);
        }

        public static TResult ReturnWithFallback<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator,
                                                                  Action failureOperation)
        {
            if (Equals(input, default(TInput)))
            {
                failureOperation();
                return default(TResult);
            }
            return evaluator(input);
        }

        public static TResult Return<TInput, TParam, TResult>(this TInput input, Func<TInput, TParam, TResult> evaluator,
                                                              TParam p, TResult failureValue)
        {
            TInput nothingInInput = default(TInput);
            TParam nothingInParam = default(TParam);
            if (Equals(input, nothingInInput) || p.Equals(nothingInParam)) return failureValue;
            return evaluator(input, p);
        }

        public static TInput If<TInput>(this TInput input, Func<TInput, bool> evaluator)
        {
            TInput nothing = default(TInput);
            if (Equals(input, nothing)) return nothing;
            return evaluator(input) ? input : nothing;
        }

        public static TInput Unless<TInput>(this TInput input, Func<TInput, bool> evaluator)
        {
            TInput nothing = default(TInput);
            if (Equals(input, nothing)) return nothing;
            return evaluator(input) ? nothing : input;
        }

        public static TInput Do<TInput>(this TInput input, Action<TInput> action)
        {
            TInput nothing = default(TInput);
            if (Equals(input, nothing)) return nothing;
            action(input);
            return input;
        }

        public static TInput Do<TInput, TParam>(this TInput input, Action<TInput, TParam> action, TParam p)
        {
            TInput nothing = default(TInput);

            if (Equals(input, nothing)) return nothing;
            action(input, p);
            return input;
        }
    }
}