﻿namespace CS341_YMCA.Data
{
    public class EndpointResultToken<T>
    {
        public bool Success { get; set; } = true;
        public string? Error { get; set; } = null;
        public T? Value { get; set; } = default;

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public T? Get()
        {
            if (!Success)
                throw new Exception(Error);

            return Value;
        }
    }
}
