# Pretzel.SimilarPosts

Allows you find similar posts for your blog and add "It's may be interested for you" links to your blog pages.

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



### Installation

Download the [latest release](https://github.com/sergeyzwezdin/Pretzel.SimilarPosts/releases/latest) and extract `Pretzel.SimilarPosts.zip` to the `_plugins` folder at the root of your site folder.