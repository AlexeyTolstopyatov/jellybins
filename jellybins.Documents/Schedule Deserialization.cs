// Десериализация расписания
//
// https://intime.tsu.ru/api/old-web/v1/schedule/group?id=c60d2e5a-1751-11ef-815c-005056bc52bb&dateFrom=2024-12-16&dateTo=2024-12-21
//
// Расписание содержит все 7 пар, не смотря на "окна"
// Сегмент списка включает объекты похожей структуры
// 
// date: "YYYY-MM-DD"
// lessons: []
//      type=string
//      id="{8}-{4}-{4}-{4}-{12}" hash
//      lessonNumber=int32
//      lessonType=string
//      title=string
//
//      groups: []
//          id=hash
//          name=string
//          isSubgroup=boolean
//      professor
//          id=hash
//          fullName=string
//      audience
//          id=hash
//          name=string
//      starts=int32
//      ends=int32

public class Professor : InTimeObject {
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("fullName")] public string Name { get; set; }
}

public class Audience : InTimeObject { }

public class Group {
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("isSubgroup")] public bool IsSubgroup { get; set; }
}

public class Lesson {
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("id")] public string? Id { get; set; }
    [JsonProperty("title")] public string? Title { get; set; } 
    [JsonProperty("starts")] public int Starts { get; set; }
    [JsonProperty("ends")] public int Ends { get; set; }
    [JsonProperty("lessonNumber")] public int LessonNumber { get; set; }
    [JsonProperty("lessonType")] public string LessonType { get; set; }
    [JsonProperty("groups")] public Group[] Groups { get; set; }
    [JsonProperty("audience")] public Audience Audience { get; set; }
    [JsonProperty("professor")] public Professor Professor { get; set; }

}

public class Day {
    public Date Date { get; set; }
    public Lesson[] Lessons { get; set; }
}

private InTimeDriver DeserializeSchedule() {
    // Собрать список: new Day[7] с фиксированными днями недели
    // на основе зафиксированных в календаре дней
    // По умолчанию рассчитать 7 дней недели (dateFrom - dateTo)
    // для недели, в которой будет сегодня (Date.Now)

    return this;
} 