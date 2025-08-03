# **LChess**

## **📗 목차**

<b>
  
- 📝 [개요]
- 🛠 [기술 및 도구]
- 📚 [라이브러리]
- 🔧 [기능 구현]
  - [홈 화면]
  - [기물 색상 선택]
  - [체스 게임]
  - [기보 선택 및 분석]
  - [사용자 설정]

</b>

## **📝 LChess 개요**
<img width="988" height="987" alt="Image" src="https://github.com/user-attachments/assets/db496c3a-72f1-4ffc-8341-9348ddb225dd" />

> **프로젝트 목적 :** 오픈소스 개발자 대회 참여, IPC 통신 학습
>
> **기획 및 제작 :** 이전석
>
> **주요 기능 :** 체스에 특화된 AI엔진인 Stockfish와 IPC 통신을 통하여 경기 및 분석기능 제공.
>
> **개발 환경 :** Windows 11, Visual Studio 2022 Community, .Net 9.0
>
> **문의 :** malbox5034@naver.com

<br/>

## **🛠 기술 및 도구**
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![VS](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual%20studio&logoColor=whit)
![GitHub](https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white)

<br />

## **📚 라이브러리**

|라이브러리|버전|비고|
|:---|---:|:---:|
|CommunityToolkit.Mvvm|8.4.0|MVVM|
|Microsoft.Extensions.DependencyInjection|9.0.6|의존성 주입|
|Microsoft.Xaml.Behaviors.Wpf|1.1.135|MVVM|
|Newtonsoft.Json|13.0.3|Json 처리|
|Serilog|7.0.0|로깅|

<br/>

## **🔧기능 구현**

### **1.홈 화면**

<img width="1919" height="1028" alt="Image" src="https://github.com/user-attachments/assets/e408adb4-60a4-45ae-8789-df2084ec4920" />

- AI 대국 : Stockfish 엔진과 체스경기를 진행. AI 판단깊이가 깊을수록 AI턴이 길어짐.
- 기보분석 : AI와 진행했던 경기를 저장하여 경기를 분석할 수 있으며, 중간 기보부터 이어하기 기능을 제공.
- 환경설정 : AI 판단깊이, 기보 저장경로 등등 환경설정을 제공하는 화면.

### **2.기물 색상 선택**

<img width="1919" height="1030" alt="Image" src="https://github.com/user-attachments/assets/37dc22db-8fa3-4d77-9540-b2cd99440075" />

- AI 대국 전, 기물 색상을 선택할 수 있는 화면.
- 체스는 백색 기물이 먼저 수를 둘 수 있음.

### **3.체스 게임**

<img width="1919" height="1030" alt="Image" src="https://github.com/user-attachments/assets/16b17840-82ce-40a3-9a6b-ecfba9ce9146" />

- 기물을 선택하면 기물이 움직일 수 있는 경로가 생성되며, 적 기물을 잡을 수 있을 경우 빨간색으로 타일이 하이라이트 됨.
- 해당 유닛이 움직였을 때 킹이 위협받게 되면 체스 룰에 어긋나는 행위이므로 기물 이동경로에서 제외된 상태로 경로가 생성됨.
- 화면이 늘어나거나 줄어듦에 따라 체스 보드는 정사각형을 유지하며 해당 비율에 따라 가로/세로 길이가 조정됨.
- 우측 대시보드를 통하여 기권하거나, 기물 이동이력을 확인할 수 있음.

### **4. 기보 선택 및 분석**

<img width="1919" height="1030" alt="Image" src="https://github.com/user-attachments/assets/48fece50-27c2-4649-9f81-bc5e243746b8" />

- 저장된 기보를 클릭하거나, 종료된 게임에서 분석화면으로 이동 시 해당 기보를 분석할 수 있음.
- 해당 화면에서는 적 기물을 클릭하여 적 기물이 움직일 수 있는 경로도 확인이 가능하며, 우측 대시보드의 기보를 클릭하면 해당 기보 상태로 보드가 변경됨.
  
### **5. 사용자 설정**

<img width="1919" height="1029" alt="Image" src="https://github.com/user-attachments/assets/8cb6ac37-43fb-426c-a546-b1cda14ffca0" />

- 판단깊이 : Stockfish 엔진이 얼만큼의 앞 수를 계산할지에 대한 설정. 해당 값이 높을수록 AI의 수가 견고해지며, 판단 시간이 오래 걸린다. (20 ~ 30 권장)
- 기보저장폴더 : 게임이 끝난 후 기보를 어느 경로에 저장 할 것인지에 대한 설정. 해당 경로에 포함된 기보가 아니더라도 기보 선택 화면에서 불러올 수 있다.

<br/>
<br/>
<br/>
<br/>
