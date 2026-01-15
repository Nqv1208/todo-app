namespace TodoApp.Domain.Content.Enums;

// Loại Block (Notion-style)
public enum BlockType
{
    // Text blocks
    Text = 0,
    Heading1 = 1,
    Heading2 = 2,
    Heading3 = 3,
    Quote = 4,
    Callout = 5,
    
    // List blocks
    BulletList = 10,
    NumberedList = 11,
    Checkbox = 12,
    Toggle = 13,
    
    // Media blocks
    Image = 20,
    Video = 21,
    File = 22,
    Embed = 23,
    
    // Advanced blocks
    Code = 30,
    Divider = 31,
    Table = 32,
    
    // Special blocks
    Todo = 40
}
