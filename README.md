# [WIP] *uninside*

Unofficial DCInside API written in C#

## 사용
```csharp
Uninside uninside = new Uninside();
await uninside.Initialize();

//
GalleryManager galleryManager = new GalleryManager(uninside);
Gallery gallery = await galleryManager.GetGallery("chaeyaena", GalleryType.Minor);

Console.WriteLine(gallery.Name);
Console.WriteLine(gallery.Description);
Console.WriteLine(gallery.Master.Name + " (" + gallery.Master.Id + ") - 매니저");

foreach (User subManager in gallery.SubManagers)
{
  Console.WriteLine(subManager.Name + " (" + subManager.Id + ") - 부매니저");
}

//
PostManager postManager = new PostManager(uninside);

(GalleryInfo galleryInfo, List<PostSnippet> postList) = await postManager.GetPostList(gallery);
Console.WriteLine(galleryInfo.DefaultNickname);

Post post = await postManager.ReadPost(gallery, "731835");

Console.WriteLine("[" + post.HeadTitle + "] " + post.Title);
Console.WriteLine(post.WriterName + " (" + post.WriterId + ")");
Console.WriteLine(post.Category);

//
CommentManager commentManager = new CommentManager(uninside);
List<Comment> commentList = await commentManager.GetCommentList(post);

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
