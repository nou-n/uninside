# [WIP] *uninside*

Unofficial DCInside API written in C#

## 사용
```csharp
Uninside uninside = new Uninside();
await uninside.Initialize();

Gallery gallery = new Gallery("chaeyaena", GalleryType.Minor, null, null);

PostManager postManager = new PostManager(uninside);
Post post = await postManager.ReadPost(gallery, "731835");

Console.WriteLine("[" + post.HeadTitle + "] " + post.Title);
Console.WriteLine(post.WriterName + " (" + post.WriterId + ")");

CommentManager commentManager = new CommentManager(uninside);
List<Comment> commentList = await commentManager.GetCommentList(gallery, post.Id);

foreach(Comment comment in commentList)
{
  Console.WriteLine(
    (comment.isReply ? "ㄴ " : "") +
    comment.WriterName + ": " + comment.Content
  );
}
```

## 참고한 프로젝트
| 프로젝트        | 설명                                   | 
|-------------------|---------------------------------------|
| [organization/KotlinInside](https://github.com/organization/KotlinInside)  | Kotlin으로 작성된 디시인사이드 비공식 API | 

## 포함된 라이브러리
- [gering/Tiny-JSON](https://github.com/gering/Tiny-JSON)
```
The MIT License (MIT)

Copyright (c) 2015 Robert Gering

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
