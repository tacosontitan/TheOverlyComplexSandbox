namespace Sandbox.Core {
    public class ModuleParameter {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DisplayElement DisplayElement { get; set; }
        public string RequestMessage { get; set; }
        public bool Required { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public object Value { get; set; }
    }
}