namespace BuildATrain.Common
{
    public static class Headers
    {
        public const string CONTENT_TYPE = "Content-Type";
        public const string CACHE_CONTROL = "Cache-Control";
        public const string CONNECTION = "Connection";
    }

    public static class HeaderValues
    {
        public static class ContentType
        {
            public const string TEXT_OR_EVENT_STREAM = "text/event-stream";
        }

        public static class CacheControl
        {
            public const string NO_CACHE = "no-cache";
        }

        public static class Connection
        {
            public const string KEEP_ALIVE = "keep-alive";
        }
    }
}
