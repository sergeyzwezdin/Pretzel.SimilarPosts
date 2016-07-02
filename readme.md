# Pretzel.SimilarPosts

Allows you find similar posts for your blog and add `It's may be interested for you` links to your blog pages.

This is a plugin for the static site generation tool [Pretzel](https://github.com/Code52/pretzel).

### Usage

To use this plugin you have to add configuration to `_config.yml`. Typical configuration could be like this:

```
similar_posts:
    filter_threshold: 0.3
    related_count: 3
    weight:
        categories: 0.1
        tags: 0.3
        title: 0.2
        text: 1
    stemmers:
      - English
      - Russian
    reserved:
      - asp.net
      - .net
      - vs.net
```

Configuration parameters are:
 - `filter_threshold` is param to define threshold for posts, it could be number from `0` to `1`.
 - `related_count` is param to define related posts count for each page.
 - `weight` is param to determine weight of different factors in comparision process. It accepts numbers from `0` to `1`. In this version the plugin is able to compare by `category`, `tag`, `title` and `text` (blog post content).
 - `stemmers` is param to define which stemmers to use to pre-process the text. The plugin uses [StemmersNet](https://stemmersnet.codeplex.com/) library for that. If you're using another language feel free to add additional languages [availeable](https://stemmersnet.codeplex.com/) at StemmersNet.
 - `reserved` is param to define reserver words whcih won't be processed. During the comparision process all non-alphanumeric symbols will be removed. It's fine for most cases. But it's wrong for some situations like `asp.net` word. If we won't add it to `reserved` list the plugin will understand it as two different words `asp` and `net`, which is wrong. So we add it to `reserved` list and plugin won't pre-process it at all.

Once we configure the plugin in `_config.yml` there will be available `page.related` variable for each page. To use it add code like this into your `post.html` file:

```html
{% if page.related.size > 0 -%}
<section class="related">
    <h4>You may be interested in</h4>
    <ul>
        {% for page in page.related -%}
        <li><a href="/{{page.url}}">{{page.title}}</a></li>
        {% endfor -%}
    </ul>
</section>
{% endif -%}

```

### Installation

Download the [latest release](https://github.com/sergeyzwezdin/Pretzel.SimilarPosts/releases/latest) and extract `Pretzel.SimilarPosts.zip` to the `_plugins` folder at the root of your site folder.