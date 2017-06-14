# TDD-HomeWork

## [Day1-題目](https://docs.google.com/spreadsheets/d/1P0nImGpaUYpP4yt_ecIM3TV9HtA8aE7ZyrflGoWuzsc/htmlview#)

|Id	| Cost | Revenue | SellPrice |  
|---|:----:|:-------:|:---------:|  
|1	|  1   |    11   |    21     |  
|2	|  2   |    12   |    22     |  
|3	|  3   |    13   |    23     |  
|4	|  4   |    14   |    24     |  
|5	|  5   |    15   |    25     |  
|6	|  6   |    16   |    26     |  
|7	|  7   |    17   |    27     |  
|8	|  8   |    18   |    28     |  
|9	|  9   |    19   |    29     |  
|10	|  10  |    20   |    30     |  
|11	|  11  |    21   |    31     | 

#### 測試範例：
| 測試項目              | 結果           |
| :-------             | :-----         | 
| 3筆一組，取Cost總和   | 6,15,24,21     |
| 4筆一組，取Revenue總和| 50,66,60       |

#### 題目解說：
1. 來源可以是「任何型別的集合」
2. 可以任意決定幾筆一組
3. 回傳每一組 Σf(x) 的集合，Σ結果型別可直接用int

#### 補充條件：
- 尋找的欄位若不存在，預期會拋 ArgumentException  
- 筆數若輸入負數或 0，預期會拋 ArgumentException
- 未來可能會新增其他欄位  
- 希望這個API可以給其他 domain entity 用，例如 Employee

對應檔案 **Day1Homework**

