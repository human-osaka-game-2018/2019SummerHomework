### 使用方法
dataを埋めて右下の実行buttonを押すとMySQLのcommandが走ります。
物理削除check boxをonにした状態で実行した場合はdatabaseから削除されます。

### 修正課題
- [x] data tableのstateが常にmodifiedにならないようにする
- [x] app configによるdatabaseの切り替え機能
- [ ] refactoring
- [ ] validation
- [x] commentをつける

### 感想
比較的に時間がかからない前期課題のイマイチなところを修正してみました
今回はobjectを別の方へ変換することが多かったのでparse methodより convertの方が
直接変換できるのでいいと思いました。
C#の型switchとobjectを用いることによって短くcodeを書く等
C++になかった部分に早く慣れてC#らしいcodeが書けるようになりたいです
