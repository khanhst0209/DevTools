using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.ETACalculator;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace ETA_Calculator;

public class ETA_Calculator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "ETA Calculator";

    public Category Category => Category.Math;

    public string Description { get; set; } = "Calculate estimated time of completion and duration based on consumption rate.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M6.5 7h11""></path><path d=""M6.5 17h11""></path><path d=""M6 20v-2a6 6 0 1 1 12 0v2a1 1 0 0 1-1 1H7a1 1 0 0 1-1-1z""></path><path d=""M6 4v2a6 6 0 1 0 12 0V4a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1z""></path></g></svg>"; public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "totalAmount",
                        label = "Amount of element to consume",
                        type = ComponentType.number.ToString(),
                        defaultValue = 100,
                        min = 1
                    },
                    new SchemaInput{
                        id = "startTime",
                        label = "The consumption started at",
                        type = ComponentType.date.ToString()
                    },
                    new SchemaInput{
                        id = "unitCount",
                        label = "Amount of unit consumed",
                        type = ComponentType.number.ToString(),
                        defaultValue = 1,
                        min = 1
                    },
                    new SchemaInput{
                        id = "unitTimeValue",
                        label = "Time span",
                        type = ComponentType.number.ToString(),
                        defaultValue = 1,
                        min = 1
                    },
                    new SchemaInput{
                        id = "unitTimeType",
                        label = "Unit of time",
                        type = ComponentType.dropdown.ToString(),
                        defaultValue = "minutes",
                        options = new List<ComponentOption>{
                            new ComponentOption { value = "seconds", label = "Seconds" },
                            new ComponentOption { value = "minutes", label = "Minutes" },
                            new ComponentOption { value = "hours", label = "Hours" }
                        }
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "durationResult",
                        label = "Total duration",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "endTimeResult",
                        label = "It will end at",
                        type = ComponentType.text.ToString()
                    }
                }
            }
        }
    };

    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<ETACalculatorInput>(json);

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);

        // Calculate total time in seconds
        double totalUnitsNeeded = myInput.totalAmount;
        double unitsPerTimeSpan = myInput.unitCount;
        double timeSpanValue = myInput.unitTimeValue;

        // Convert unit time to seconds for calculation
        double secondsPerUnit = timeSpanValue * GetSecondsMultiplier(myInput.unitTimeType);

        // Calculate total duration in seconds
        double totalDurationSeconds = (totalUnitsNeeded / unitsPerTimeSpan) * secondsPerUnit;

        // Format duration string
        string durationResult = FormatDuration(totalDurationSeconds);

        // Parse start time and calculate end time
        DateTime startTime;
        string endTimeResult = "";

        if (DateTime.TryParse(myInput.startTime, out startTime))
        {
            DateTime endTime = startTime.AddSeconds(totalDurationSeconds);
            endTimeResult = FormatEndTime(endTime);
        }

        return new
        {
            durationResult,
            endTimeResult
        };
    }

    private double GetSecondsMultiplier(string unitTimeType)
    {
        return unitTimeType switch
        {
            "seconds" => 1,
            "minutes" => 60,
            "hours" => 3600,
            _ => 60 // Default to minutes
        };
    }

    private string FormatDuration(double totalSeconds)
    {
        TimeSpan duration = TimeSpan.FromSeconds(totalSeconds);

        List<string> parts = new List<string>();

        if (duration.Days > 0)
            parts.Add($"{duration.Days} day{(duration.Days != 1 ? "s" : "")}");

        if (duration.Hours > 0)
            parts.Add($"{duration.Hours} hour{(duration.Hours != 1 ? "s" : "")}");

        if (duration.Minutes > 0)
            parts.Add($"{duration.Minutes} minute{(duration.Minutes != 1 ? "s" : "")}");

        if (duration.Seconds > 0 && duration.Days == 0 && duration.Hours == 0)
            parts.Add($"{duration.Seconds} second{(duration.Seconds != 1 ? "s" : "")}");

        if (parts.Count == 0)
            return "less than a second";

        return string.Join(" ", parts);
    }

    private string FormatEndTime(DateTime endTime)
    {
        DateTime now = DateTime.Now;
        string dayPrefix = "";

        if (endTime.Date == now.Date)
            dayPrefix = "today at ";
        else if (endTime.Date == now.Date.AddDays(1))
            dayPrefix = "tomorrow at ";
        else
            dayPrefix = endTime.ToString("MMM d") + " at ";

        return dayPrefix + endTime.ToString("H:mm");
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}