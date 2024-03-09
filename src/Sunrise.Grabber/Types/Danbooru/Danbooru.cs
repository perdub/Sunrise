
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Danbooru
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("uploader_id")]
        public long UploaderId { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("source")]
        public Uri Source { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("last_comment_bumped_at")]
        public object LastCommentBumpedAt { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }

        [JsonProperty("image_width")]
        public long ImageWidth { get; set; }

        [JsonProperty("image_height")]
        public long ImageHeight { get; set; }

        [JsonProperty("tag_string")]
        public string TagString { get; set; }

        [JsonProperty("fav_count")]
        public long FavCount { get; set; }

        [JsonProperty("file_ext")]
        public string FileExt { get; set; }

        [JsonProperty("last_noted_at")]
        public object LastNotedAt { get; set; }

        [JsonProperty("parent_id")]
        public object ParentId { get; set; }

        [JsonProperty("has_children")]
        public bool HasChildren { get; set; }

        [JsonProperty("approver_id")]
        public object ApproverId { get; set; }

        [JsonProperty("tag_count_general")]
        public long TagCountGeneral { get; set; }

        [JsonProperty("tag_count_artist")]
        public long TagCountArtist { get; set; }

        [JsonProperty("tag_count_character")]
        public long TagCountCharacter { get; set; }

        [JsonProperty("tag_count_copyright")]
        public long TagCountCopyright { get; set; }

        [JsonProperty("file_size")]
        public long FileSize { get; set; }

        [JsonProperty("up_score")]
        public long UpScore { get; set; }

        [JsonProperty("down_score")]
        public long DownScore { get; set; }

        [JsonProperty("is_pending")]
        public bool IsPending { get; set; }

        [JsonProperty("is_flagged")]
        public bool IsFlagged { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("tag_count")]
        public long TagCount { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("is_banned")]
        public bool IsBanned { get; set; }

        [JsonProperty("pixiv_id")]
        public object PixivId { get; set; }

        [JsonProperty("last_commented_at")]
        public object LastCommentedAt { get; set; }

        [JsonProperty("has_active_children")]
        public bool HasActiveChildren { get; set; }

        [JsonProperty("bit_flags")]
        public long BitFlags { get; set; }

        [JsonProperty("tag_count_meta")]
        public long TagCountMeta { get; set; }

        [JsonProperty("has_large")]
        public bool HasLarge { get; set; }

        [JsonProperty("has_visible_children")]
        public bool HasVisibleChildren { get; set; }

        [JsonProperty("media_asset")]
        public MediaAsset MediaAsset { get; set; }

        [JsonProperty("tag_string_general")]
        public string TagStringGeneral { get; set; }

        [JsonProperty("tag_string_character")]
        public string TagStringCharacter { get; set; }

        [JsonProperty("tag_string_copyright")]
        public string TagStringCopyright { get; set; }

        [JsonProperty("tag_string_artist")]
        public string TagStringArtist { get; set; }

        [JsonProperty("tag_string_meta")]
        public string TagStringMeta { get; set; }

        [JsonProperty("file_url")]
        public Uri FileUrl { get; set; }

        [JsonProperty("large_file_url")]
        public Uri LargeFileUrl { get; set; }

        [JsonProperty("preview_file_url")]
        public Uri PreviewFileUrl { get; set; }
    }

    public partial class MediaAsset
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("file_ext")]
        public string FileExt { get; set; }

        [JsonProperty("file_size")]
        public long FileSize { get; set; }

        [JsonProperty("image_width")]
        public long ImageWidth { get; set; }

        [JsonProperty("image_height")]
        public long ImageHeight { get; set; }

        [JsonProperty("duration")]
        public object Duration { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("file_key")]
        public string FileKey { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("pixel_hash")]
        public string PixelHash { get; set; }

        [JsonProperty("variants")]
        public Variant[] Variants { get; set; }
    }

    public partial class Variant
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("file_ext")]
        public string FileExt { get; set; }
    }

    public partial class Danbooru
    {
        public static Danbooru FromJson(string json) => JsonConvert.DeserializeObject<Danbooru>(json);
    }
