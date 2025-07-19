# JWT認証機能の準備
## 1.SecretKeyの生成
### 1-1.SecretKeyの概要
SecretKey（シークレットキー）は、JWT（JSON Web Token）の署名と検証に使う秘密鍵であり、認証の信頼性・改ざん防止の要となる非常に重要な要素
### 1-2.SecretKeyの生成
1. Linux / macOS / WSL で生成
```bash
# 32バイトのランダムキーをBase64で出力（256ビット強度）
openssl rand -base64 32

EsgwXi51YiNOw2bKPy+DDE7qz0Yq0EablZIrMB5Zb3w=
```
### 1-3. appsettings.json に設定
```json
"JwtSettings": {
  "Issuer": "RestAPI_Sample",
  "Audience": "RestAPI_Client",
  "SecretKey": "EsgwXi51YiNOw2bKPy+DDE7qz0Yq0EablZIrMB5Zb3w=",
  "ExpiresInMinutes": 60
}
```
| 設定項目           | 説明                                                                                     | 例・補足                                                  |
|--------------------|------------------------------------------------------------------------------------------|------------------------------------------------------------|
| `Issuer`           | トークンの**発行者（認証サーバ）**を示す識別名。クライアントやリソースサーバが検証に使う | 例：`"RestAPI_Sample"`<br>（アプリ名やサービス名など）     |
| `Audience`         | トークンの**対象者（使う側）**。アクセスを許可するクライアントやアプリの識別子           | 例：`"RestAPI_Client"`<br>（Webフロントやモバイルなど）   |
| `SecretKey`        | JWTに**署名・検証**を行うための**秘密鍵**。安全なランダム文字列で、外部に漏れないよう注意 | 例：`"WcZ3...sg8="`<br>（Base64形式が一般的）              |
| `ExpiresInMinutes` | トークンの**有効期限（分）**。これを超えるとトークンは無効になる                         | 例：`60` → 60分間有効                                     |
