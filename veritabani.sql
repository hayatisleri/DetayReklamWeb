IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712104420_InitialCreate'
)
BEGIN
    CREATE TABLE [Haberler] (
        [Id] int NOT NULL IDENTITY,
        [Baslik] nvarchar(200) NOT NULL,
        [SeoUrl] nvarchar(250) NOT NULL,
        [Ozet] nvarchar(500) NOT NULL,
        [Icerik] nvarchar(max) NOT NULL,
        [ResimAdresi] nvarchar(500) NOT NULL,
        [AktifMi] bit NOT NULL,
        [OlusturulmaTarihi] datetime2 NOT NULL,
        CONSTRAINT [PK_Haberler] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712104420_InitialCreate'
)
BEGIN
    CREATE TABLE [Hizmetler] (
        [Id] int NOT NULL IDENTITY,
        [Baslik] nvarchar(200) NOT NULL,
        [KisaAciklama] nvarchar(500) NOT NULL,
        [Icerik] nvarchar(max) NOT NULL,
        [ResimAdresi] nvarchar(500) NOT NULL,
        [Sira] int NOT NULL,
        [AktifMi] bit NOT NULL,
        [OlusturulmaTarihi] datetime2 NOT NULL,
        CONSTRAINT [PK_Hizmetler] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712104420_InitialCreate'
)
BEGIN
    CREATE TABLE [IletisimMesajlari] (
        [Id] int NOT NULL IDENTITY,
        [AdSoyad] nvarchar(150) NOT NULL,
        [Eposta] nvarchar(150) NOT NULL,
        [Telefon] nvarchar(50) NOT NULL,
        [Konu] nvarchar(200) NOT NULL,
        [Mesaj] nvarchar(max) NOT NULL,
        [OkunduMu] bit NOT NULL,
        [GonderilmeTarihi] datetime2 NOT NULL,
        CONSTRAINT [PK_IletisimMesajlari] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712104420_InitialCreate'
)
BEGIN
    CREATE TABLE [Sayfalar] (
        [Id] int NOT NULL IDENTITY,
        [Baslik] nvarchar(200) NOT NULL,
        [SeoUrl] nvarchar(250) NOT NULL,
        [Icerik] nvarchar(max) NOT NULL,
        [AktifMi] bit NOT NULL,
        [SeoBasligi] nvarchar(200) NOT NULL,
        [SeoAciklamasi] nvarchar(500) NOT NULL,
        [OlusturulmaTarihi] datetime2 NOT NULL,
        CONSTRAINT [PK_Sayfalar] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712104420_InitialCreate'
)
BEGIN
    CREATE TABLE [SiteAyarlari] (
        [Id] int NOT NULL IDENTITY,
        [SiteBasligi] nvarchar(100) NOT NULL,
        [SiteAciklamasi] nvarchar(250) NOT NULL,
        [LogoAdresi] nvarchar(500) NOT NULL,
        [FaviconAdresi] nvarchar(500) NOT NULL,
        [TelefonNumarasi] nvarchar(100) NOT NULL,
        [EpostaAdresi] nvarchar(100) NOT NULL,
        [Adres] nvarchar(250) NOT NULL,
        [FacebookAdresi] nvarchar(250) NOT NULL,
        [InstagramAdresi] nvarchar(250) NOT NULL,
        [LinkedInAdresi] nvarchar(250) NOT NULL,
        [GuncellenmeTarihi] datetime2 NOT NULL,
        CONSTRAINT [PK_SiteAyarlari] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712104420_InitialCreate'
)
BEGIN
    CREATE TABLE [Slaytlar] (
        [Id] int NOT NULL IDENTITY,
        [Baslik] nvarchar(200) NOT NULL,
        [AltBaslik] nvarchar(200) NOT NULL,
        [ResimAdresi] nvarchar(500) NOT NULL,
        [LinkAdresi] nvarchar(500) NOT NULL,
        [Sira] int NOT NULL,
        [AktifMi] bit NOT NULL,
        CONSTRAINT [PK_Slaytlar] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712104420_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260712104420_InitialCreate', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712143615_YeniModelleriEkle'
)
BEGIN
    CREATE TABLE [Galeriler] (
        [Id] int NOT NULL IDENTITY,
        [Baslik] nvarchar(200) NOT NULL,
        [ResimAdresi] nvarchar(500) NOT NULL,
        [Sira] int NOT NULL,
        CONSTRAINT [PK_Galeriler] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712143615_YeniModelleriEkle'
)
BEGIN
    CREATE TABLE [IsBasvurulari] (
        [Id] int NOT NULL IDENTITY,
        [AdSoyad] nvarchar(150) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [Pozisyon] nvarchar(max) NOT NULL,
        [Mesaj] nvarchar(max) NOT NULL,
        [CvDosyaYolu] nvarchar(max) NOT NULL,
        [BasvuruTarihi] datetime2 NOT NULL,
        CONSTRAINT [PK_IsBasvurulari] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712143615_YeniModelleriEkle'
)
BEGIN
    CREATE TABLE [Projeler] (
        [Id] int NOT NULL IDENTITY,
        [Baslik] nvarchar(200) NOT NULL,
        [Icerik] nvarchar(max) NOT NULL,
        [KapakResmi] nvarchar(500) NOT NULL,
        [AktifMi] bit NOT NULL,
        CONSTRAINT [PK_Projeler] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712143615_YeniModelleriEkle'
)
BEGIN
    CREATE TABLE [Urunler] (
        [Id] int NOT NULL IDENTITY,
        [Ad] nvarchar(200) NOT NULL,
        [Aciklama] nvarchar(max) NOT NULL,
        [ResimAdresi] nvarchar(500) NOT NULL,
        [TeknikDetaylar] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Urunler] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712143615_YeniModelleriEkle'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260712143615_YeniModelleriEkle', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712165756_AddAnalyticsToSiteAyari'
)
BEGIN
    ALTER TABLE [SiteAyarlari] ADD [GoogleAnalyticsId] nvarchar(50) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712165756_AddAnalyticsToSiteAyari'
)
BEGIN
    ALTER TABLE [SiteAyarlari] ADD [LookerStudioUrl] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712165756_AddAnalyticsToSiteAyari'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260712165756_AddAnalyticsToSiteAyari', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712174955_AddEkipUyeleri'
)
BEGIN
    CREATE TABLE [EkipUyeleri] (
        [Id] int NOT NULL IDENTITY,
        [AdSoyad] nvarchar(150) NOT NULL,
        [Unvan] nvarchar(150) NOT NULL,
        [ResimAdresi] nvarchar(max) NOT NULL,
        [LinkedInAdresi] nvarchar(250) NOT NULL,
        [TwitterAdresi] nvarchar(250) NOT NULL,
        [InstagramAdresi] nvarchar(250) NOT NULL,
        [Sira] int NOT NULL,
        [AktifMi] bit NOT NULL,
        CONSTRAINT [PK_EkipUyeleri] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712174955_AddEkipUyeleri'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260712174955_AddEkipUyeleri', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712191806_menu_ayar_eklenmesi'
)
BEGIN
    CREATE TABLE [Menuler] (
        [Id] int NOT NULL IDENTITY,
        [Baslik] nvarchar(100) NOT NULL,
        [Url] nvarchar(250) NOT NULL,
        [Sira] int NOT NULL,
        [UstMenuId] int NULL,
        [AktifMi] bit NOT NULL,
        CONSTRAINT [PK_Menuler] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712191806_menu_ayar_eklenmesi'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260712191806_menu_ayar_eklenmesi', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712201015_admin_Kullanicisi_olusturma'
)
BEGIN
    CREATE TABLE [Kullanicilar] (
        [Id] int NOT NULL IDENTITY,
        [KullaniciAdi] nvarchar(max) NOT NULL,
        [SifreHash] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Kullanicilar] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260712201015_admin_Kullanicisi_olusturma'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260712201015_admin_Kullanicisi_olusturma', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714194926_UpdateSiteAyariGsc'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SiteAyarlari]') AND [c].[name] = N'LookerStudioUrl');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [SiteAyarlari] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [SiteAyarlari] DROP COLUMN [LookerStudioUrl];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714194926_UpdateSiteAyariGsc'
)
BEGIN
    ALTER TABLE [SiteAyarlari] ADD [GscPropertyUrl] nvarchar(250) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714194926_UpdateSiteAyariGsc'
)
BEGIN
    ALTER TABLE [SiteAyarlari] ADD [GscServiceAccountJsonPath] nvarchar(500) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714194926_UpdateSiteAyariGsc'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260714194926_UpdateSiteAyariGsc', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916075317_AddReferansTable'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Hizmetler]') AND [c].[name] = N'ResimAdresi');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Hizmetler] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Hizmetler] ALTER COLUMN [ResimAdresi] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916075317_AddReferansTable'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Hizmetler]') AND [c].[name] = N'KisaAciklama');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Hizmetler] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Hizmetler] ALTER COLUMN [KisaAciklama] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916075317_AddReferansTable'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Hizmetler]') AND [c].[name] = N'Icerik');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Hizmetler] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Hizmetler] ALTER COLUMN [Icerik] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916075317_AddReferansTable'
)
BEGIN
    CREATE TABLE [Referanslar] (
        [Id] int NOT NULL IDENTITY,
        [FirmaAdi] nvarchar(150) NOT NULL,
        [LogoAdresi] nvarchar(250) NULL,
        [Sira] int NOT NULL,
        [AktifMi] bit NOT NULL,
        CONSTRAINT [PK_Referanslar] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916075317_AddReferansTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260916075317_AddReferansTable', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260918075759_OneCikanKutularEklendi'
)
BEGIN
    CREATE TABLE [OneCikanKutular] (
        [Id] int NOT NULL IDENTITY,
        [Baslik] nvarchar(100) NOT NULL,
        [Ikon] nvarchar(100) NULL,
        [Link] nvarchar(250) NULL,
        [Sira] int NOT NULL,
        [AktifMi] bit NOT NULL,
        CONSTRAINT [PK_OneCikanKutular] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260918075759_OneCikanKutularEklendi'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260918075759_OneCikanKutularEklendi', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921082710_MusteriYorumuEklendi'
)
BEGIN
    CREATE TABLE [MusteriYorumlari] (
        [Id] int NOT NULL IDENTITY,
        [AdSoyad] nvarchar(100) NOT NULL,
        [Unvan] nvarchar(100) NULL,
        [Yorum] nvarchar(max) NOT NULL,
        [Sira] int NOT NULL,
        [AktifMi] bit NOT NULL,
        CONSTRAINT [PK_MusteriYorumlari] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921082710_MusteriYorumuEklendi'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260921082710_MusteriYorumuEklendi', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921093208_KategoriAlanlariEklendi'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Urunler]') AND [c].[name] = N'TeknikDetaylar');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Urunler] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Urunler] ALTER COLUMN [TeknikDetaylar] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921093208_KategoriAlanlariEklendi'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Urunler]') AND [c].[name] = N'ResimAdresi');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Urunler] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Urunler] ALTER COLUMN [ResimAdresi] nvarchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921093208_KategoriAlanlariEklendi'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Urunler]') AND [c].[name] = N'Aciklama');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Urunler] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [Urunler] ALTER COLUMN [Aciklama] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921093208_KategoriAlanlariEklendi'
)
BEGIN
    ALTER TABLE [Urunler] ADD [Kategori] nvarchar(100) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921093208_KategoriAlanlariEklendi'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Projeler]') AND [c].[name] = N'KapakResmi');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Projeler] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Projeler] ALTER COLUMN [KapakResmi] nvarchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921093208_KategoriAlanlariEklendi'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Projeler]') AND [c].[name] = N'Icerik');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Projeler] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Projeler] ALTER COLUMN [Icerik] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921093208_KategoriAlanlariEklendi'
)
BEGIN
    ALTER TABLE [Projeler] ADD [Kategori] nvarchar(100) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260921093208_KategoriAlanlariEklendi'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260921093208_KategoriAlanlariEklendi', N'8.0.28');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260922143200_AddWhatsappNumarasiToSiteAyar'
)
BEGIN
    ALTER TABLE [SiteAyarlari] ADD [WhatsappNumarasi] nvarchar(250) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260922143200_AddWhatsappNumarasiToSiteAyar'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260922143200_AddWhatsappNumarasiToSiteAyar', N'8.0.28');
END;
GO

COMMIT;
GO

