# Unity Localize Text

Unityのテキストのローカライズライブラリです.

文字列の管理する用途のみでも, 利用することができます.# Overview

GoogleSpreadSheetから, テキストローカライズに特化したScriptableObject(TextSet)を生成します![Sample Data SS](https://i.gyazo.com/9f435e3fc5022570b560836b3675a5b2.png)## TextSet
```csharpTextSet localizedTitle = SingletonTextSet.Instance.Text(TextSetKey.title);Debug.Log(localizedTitle);
//SystemLanguageがUS Localized Localized Text Sample App
//SystemLanguageがJapanese Localized Text サンプルアプリ
```

Google Spread Sheet Sample Data (Read Only)[https://docs.google.com/spreadsheets/d/1jB_F3j0umUc522B66OKHMZFBAG2mTnoOnGgkfccTJIA/edit#gid=0]()
# 動作モード
動作モードは`SingletonSetting`, `AdvancedSetting`の2つがあります

- **SingletonSetting:** ResoucesフォルダにScriptableObjectを生成して使います
- **AdvancedSetting:** Settingsで指定した場所にScriptableObjectを生成し好みに応じて使います

# SingletonSetting設定方法

おおまかな流れ

1. UnityのアセットメニューからSettingsを生成する
2. Settingsに設定を行う
3. SettingsからTextSetを生成する

![065b4f6d00146d371a14e7c81801b98c](https://user-images.githubusercontent.com/7759549/43685023-4c0e65c2-98e6-11e8-850d-b95727270438.png)詳細な流れ

1. `LocalText`に準拠した`GoogleSpreadSheet`を作成する `Sample google spreadsheet`を複製すると良い

[Sample google spreadsheet](https://docs.google.com/spreadsheets/d/e/2PACX-1vRVG09sHjgpAKLrC4gK7tr4dKlm0CTi8jOy1E8tLqb9_gAvEiRt4_rprcjsRLGv5mGXW6c7tWbWz0m0/pub?gid=0&single=true&output=tsv)

- 1行目コメント行なので何を書いても書かなくてもOKです. 実データは2行目からなので1行目が存在しないと問題が発生します
- 1列目 キー文字列です。C#上の`Dictionary`のキーとなる文字列です
- 2列目 コメント行ですC#のコード補完で表示されます
- 3列目 ~ N列目　キーに対応するローカライズされたテキストです. 例では３列目は英語, ４列目はスペイン語 5列目は日本語になります. 

※追加できる言語は、Unityの`SystemLanguage`に定義されている言語になります

[https://docs.unity3d.com/ja/540/ScriptReference/SystemLanguage.html](https://docs.unity3d.com/ja/540/ScriptReference/SystemLanguage.html)

2. 設定情報オブジェクト`Settingオブジェクト`を作る


- `Asset Menu　/ Lozalize Text / Create Singleton Settings`を選択し`LocalTextSettings`オブジェクトを作成します.

![](https://i.gyazo.com/011cdf122de695546bc996b3924f56de.png)

- `Settingオブジェクト`が作成できたら、インスペクタから必要な情報を入力します。Singletonモードでの必要な情報は下記になります。
	- SpreadSheetのURL
	- サポートする言語の追加
	- デフォルト言語

- Languageの項目のプラスボタンから追加し, デフォルトの言語にチェックを付け下記のようにします
![](https://i.gyazo.com/96994d16e7019634a08740625deb49a0.png)

- Google Spread SheetのファイルメニューからWebに公開を選択し、形式をタブ区切りを選択し、下記のように設定しURLを取得します

![](https://i.gyazo.com/c45cefff7aef832d3f4c30e9f48b2d26.png)

- URLを取得したら設定オブジェクトに入力します
- `Settingオブジェクト`のインスペクタの`Create`をおすと`LocalizedText/Resources`以下にTextSetが生成されます.

※Resourcesを使いたくない人は`Advanced Setting`からより細かい設定のScriptableObjectが作成できるので、そちらのモードで利用してください

![](https://i.gyazo.com/c8c400194c5521f650a40421a9652543.png)

- TextSetのインスペクタからテキストが反映されていることを確認し、設定は完了です。
![](https://i.gyazo.com/e96885760ea235574209feefd3c9305a.png)

- **SingletonSetting**だと自動でシステム言語の変更が反映されます。
- Text中の`<br>`タグを改行コードに変換します

## Unity Version
- Unity5以降
- Unity2017.4で動作確認しています

## Version
- N/A

## License
MIT___

# リファレンス

# class TextSet

## `CurrentSystemLanguage`

```csharp
public SystemLanguage CurrentSystemLanguage        
```
プロパティ
LocalizeTextで設定されいる言語を設定, 取得します

## `this[string key]`
```csharp
public string this
```
インデクサ 現在の言語の文字列を取得します
Textメソッドと等価

```
//例
textSet["title"];
```

## `Text`

```csharp
public string Text(string key)
```      
現在の言語の文字列を取得します

## `Format`
```csharp
public string Format(string key, params object[] parasmsObjects)
```  
フォーマットを指定して文字列を取得します。
```
textSet.Format("price{0}", "100円");
```

# class SingletonTextSet

## `Instance`
```csharp
public static TextSet Instance
```
`Resources`にある`ScriptableObject`TextSetのシングルトンを取得します


