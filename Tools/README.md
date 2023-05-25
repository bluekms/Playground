# Excel To Csv With Library (E2CF)

## Outline

Excel 파일의 특정 시트명으로 데이터를 가져온다.
string으로 제공되는 Record Class의 맴버변수 이름 (혹은 Attribute에 나와있는 이름)을 토대로
읽어갈 수 있는 부분만 csv 파일로 출력한다.

Unity Client를 위함
Unity Client 라이브러리를 같은 프로젝트에 포함시키는게 더 좋은가?

# Excel To Csv With Library (E2CL)

## Outline

Excel 파일의 특정 시트명으로 Library에서 Record 클래스를 찾아온다.
해당 클래스의 맴버변수 이름 (혹은 Attribute에 나와있는 이름)을 토대로 읽어갈 수 있는 부분만 csv 파일로 출력한다.

## Arguments
* -e, --excel: 옵션. 단 하나의 액셀 파일. directory와 함께 사용할 수 없습니다.
* -s, --sheet: 옵션. 액셀 파일 하나의 특정 시트를 지정. file과 함께 사용될 수 있습니다. directory라면 무시됩니다.
* -d, --directory: 옵션. 액셀 파일들이 있는 디렉터리. file과 함께 사용될 수 없습니다.
* -o, --output: 필수. csv 파일 출력 경로
* -t, --test: 옵션. 해당 인자값이 있다면 TestStaticDataContext 에서 Record 클래스를 찾아옵니다.

```
-e D:\Workspace\gitProject\Playground\StaticData\__TestStaticData\__TestStaticData.xlsx
-o D:\Workspace\gitProject\Playground\StaticData\__TestStaticData\Output
-t
```

```
-e D:\Workspace\gitProject\Playground\StaticData\__TestStaticData\__TestStaticData.xlsx
-s TargetTest
-o D:\Workspace\gitProject\Playground\StaticData\__TestStaticData\Output
-t
```


# CsvLoadTester

Csv