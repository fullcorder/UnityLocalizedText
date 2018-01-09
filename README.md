# Unity Localize Text

Unityのテキストのローカライズライブラリです

GoogleSpreadSheetから, テキストローカライズに特化したScriptableObject(TextSet)を生成します

//SystemLanguageがUS Localized Localized Text Sample App
//SystemLanguageがJapanese Localized Text サンプルアプリ
```

Google Spread Sheet Sample Data (Read Only)

## モード
- お手軽なResourcesを使うSingletonモードがあります
- より詳細な設定ができ, ScriptableObjectの生成までのAdvanceモードがあります. AdvanceモードはSingletonではないため複数のTextSetを持つことができます

## Unity Version
- Unity5以降
- Unity5.5, Unity2017.1で動作確認しています

## Version
- N/A

## License
MIT

1. `LocalText`に準拠した`GoogleSpreadSheet`を作成する

- 1行目コメント行なので何を書いても書かなくてもOKです. 実データは2行目からなので1行目が存在しないと問題が発生します
- 1列目 キー文字列です。C#上の`Dictionary`のキーとなる文字列です
- 2列目 コメント行ですC#のコード補完で表示されます
- 3列目 ~ N列目　キーに対応するローカライズされたテキストです. ３列目は英語, ４列目はスペイン語 5列目は日本語になります. 追加できる言語は、Unityの`SystemLanguage`に定義されている言語になります

[https://docs.unity3d.com/ja/540/ScriptReference/SystemLanguage.html](https://docs.unity3d.com/ja/540/ScriptReference/SystemLanguage.html)

2. Unity側で設定情報を持つ`Settingオブジェクト`を作る

- `LocalText`は`Singleton`以外でも利用するこも可能ですが, シングルトンでの使い方を説明します

- `Asset Menu　/ Lozalize Text / Create Singleton Settings`を選択し`LocalTextSettings`オブジェクトを作成します。これは`Editor`以下のディレクトリが良いです

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
- `Settingオブジェクト`のインスペクタの`Create`をおすと`LocalizedText/Resources`以下にTextSetが生成されます。Resourcesを使いたくない人は`Advanced Setting`からより細かい設定のScriptableObjectが作成できるので、そちらのモードで利用してください
![](https://i.gyazo.com/c8c400194c5521f650a40421a9652543.png)

- TextSetのインスペクタからテキストが反映されていることを確認し、設定は完了です。
![](https://i.gyazo.com/e96885760ea235574209feefd3c9305a.png)

- なおSingletonモードだと自動でシステム言語の変更が反映されます。また、スクリプトから変更することも可能です

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
