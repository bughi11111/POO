using Proiect_POO;
using Proiect.POO; // Folosim modelele unificate
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private StoryDefinition? _currentStory;
    private StoryBlock? _currentBlock;
    private Dictionary<string, double> _gameState = new();

    public Form1()
    {
        InitializeComponent();
    }

    private void btnLoadStory_Click(object sender, EventArgs e) => OpenStoryFile();

    private void OpenStoryFile()
    {
        using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Story Files (*.zip;*.json)|*.zip;*.json" })
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Dacă e ZIP, folosim Repository, dacă e JSON, citim direct
                    if (ofd.FileName.EndsWith(".zip"))
                         _currentStory = StoryRepository.Load(ofd.FileName); 
                    else
                    {
                        string json = System.IO.File.ReadAllText(ofd.FileName);
                        _currentStory = System.Text.Json.JsonSerializer.Deserialize<StoryDefinition>(json,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }

                    if (_currentStory != null)
                    { 
                        InitializeGameState();
                        GoToBlock(_currentStory.StartBlock); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la încărcare: " + ex.Message);
                }
            }
        }
    }

    private void InitializeGameState()
    {
        _gameState.Clear();
        if (_currentStory == null) return;
        foreach (var prop in _currentStory.Properties)
             _gameState[prop.Key] = prop.Initial; 
    }

    private void GoToBlock(string blockId)
    {
        if (_currentStory == null) return;

        _currentBlock = _currentStory.Blocks.FirstOrDefault(b => b.Id == blockId);
        if (_currentBlock == null) return;

         richAfisare.Text = _currentBlock.Text;
         UpdateHUD(); 
         GenerateDecisions(); 
    }

    private void GenerateDecisions()
    {
        pnlDecisions.Controls.Clear();
        if (_currentBlock?.Decisions == null) return;

        foreach (var decision in _currentBlock.Decisions)
        {
            // Filtrare decizii pe baza condițiilor AST [cite: 78, 205]
            if (EvaluateCondition(decision.Condition))
            {
                Button btn = new Button { Text = decision.Text, AutoSize = true, Tag = decision };
                btn.Click += (s, e) => {
                    var d = (DecisionDefinition)((Button)s!).Tag!;
                    ApplyEffects(d.Effects); 
                    GoToBlock(d.TargetBlock); 
                };
                 pnlDecisions.Controls.Add(btn); 
            }
        }
    }

    
    private bool EvaluateCondition(ConditionNode? node)
    {
        if (node == null) return true;

        if (node is ComparisonNode comp)
        {
            if (!_gameState.ContainsKey(comp.Property)) return false;
            double current = _gameState[comp.Property];
            return comp.Operator switch
            {
                "==" => current == comp.Value,
                "!=" => current != comp.Value,
                ">" => current > comp.Value,
                ">=" => current >= comp.Value,
                "<" => current < comp.Value,
                "<=" => current <= comp.Value,
                _ => false
            };
        }
         if (node is AndNode andNode) return andNode.Conditions.All(EvaluateCondition);
         if (node is OrNode orNode) return orNode.Conditions.Any(EvaluateCondition); 
        return false;
    }

    private void ApplyEffects(List<EffectDefinition>? effects)
    {
        if (effects == null || _currentStory == null) return;

        foreach (var effect in effects)
        {
            if (!_gameState.ContainsKey(effect.Property)) continue;

             if (effect.Type == "ADD") _gameState[effect.Property] += effect.Value; 
            else if (effect.Type == "SET") _gameState[effect.Property] = effect.Value; 

            var def = _currentStory.Properties.First(p => p.Key == effect.Property);
             _gameState[effect.Property] = Math.Clamp(_gameState[effect.Property], def.Min, def.Max);

            
            if (_gameState[effect.Property] <= def.Min && !string.IsNullOrEmpty(def.OnMinBlock))
            {
                GoToBlock(def.OnMinBlock);
                return;
            }
            if (_gameState[effect.Property] >= def.Max && !string.IsNullOrEmpty(def.OnMaxBlock))
            {
                GoToBlock(def.OnMaxBlock);
                return;
            }
        }
    }

    private void UpdateHUD()
    {
        if (_currentStory == null) return;
        var visibleStats = _currentStory.Properties
            .Where(p => p.VisibleInHud)
            .OrderBy(p => p.HudOrder)
            .Select(p => $"{p.HudLabel}: {_gameState[p.Key]}");

        this.Text = $"{_currentStory.Title} | " + string.Join(" | ", visibleStats); 
    }
}
