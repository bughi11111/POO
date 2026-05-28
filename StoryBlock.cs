using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Proiect.POO
{
    public class StoryDefinition
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";
        [JsonPropertyName("startBlock")]
        public string StartBlock { get; set; } = "";
        [JsonPropertyName("properties")]
        public List<StatePropertyDefinition> Properties { get; set; } = new();
        [JsonPropertyName("blocks")]
        public List<StoryBlock> Blocks { get; set; } = new();
    }

    public class StatePropertyDefinition
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = "";
        [JsonPropertyName("hudLabel")]
        public string HudLabel { get; set; } = "";
        [JsonPropertyName("min")]
        public double Min { get; set; }
        [JsonPropertyName("max")]
        public double Max { get; set; }
        [JsonPropertyName("initial")]
        public double Initial { get; set; }
        [JsonPropertyName("visibleInHud")]
        public bool VisibleInHud { get; set; }
        [JsonPropertyName("hudOrder")]
        public int HudOrder { get; set; }
        [JsonPropertyName("onMinBlock")]
        public string? OnMinBlock { get; set; }
        [JsonPropertyName("onMaxBlock")]
        public string? OnMaxBlock { get; set; }
    }

    public class StoryBlock
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";
        [JsonPropertyName("text")]
        public string Text { get; set; } = "";
        [JsonPropertyName("isFinal")]
        public bool IsFinal { get; set; }
        [JsonPropertyName("backgroundImage")]
        public string? BackgroundImage { get; set; }
        [JsonPropertyName("decisions")]
        public List<DecisionDefinition> Decisions { get; set; } = new();
    }

    public class DecisionDefinition
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = "";
        [JsonPropertyName("targetBlock")]
        public string TargetBlock { get; set; } = "";
        [JsonPropertyName("icon")]
        public string? Icon { get; set; }
        [JsonPropertyName("condition")]
        public ConditionNode? Condition { get; set; }
        [JsonPropertyName("effects")]
        public List<EffectDefinition> Effects { get; set; } = new();
    }

    public class EffectDefinition
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "ADD";
        [JsonPropertyName("property")]
        public string Property { get; set; } = "";
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(ComparisonNode), "COMPARISON")]
    [JsonDerivedType(typeof(AndNode), "AND")]
    [JsonDerivedType(typeof(OrNode), "OR")]
    public abstract class ConditionNode { }

    public class ComparisonNode : ConditionNode
    {
        [JsonPropertyName("property")]
        public string Property { get; set; } = "";
        [JsonPropertyName("operator")]
        public string Operator { get; set; } = "==";
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }

    public class AndNode : ConditionNode
    {
        [JsonPropertyName("conditions")]
        public List<ConditionNode> Conditions { get; set; } = new();
    }

    public class OrNode : ConditionNode
    {
        [JsonPropertyName("conditions")]
        public List<ConditionNode> Conditions { get; set; } = new();
    }
}
