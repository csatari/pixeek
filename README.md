# pixeek
## 1.	Követelmény feltárás

### 1.1 Célkitűzés

A program egy hidden object stílusú képkereső játék. A játékmenet folyamán négyzetekre osztott pályákon kell megnevezett objektumok képeit megtalálni. A játék célközönsége a casual gamerek.

A tárgy első féléves keretein belül a cél egy gameplay prototype elkészítése, amit aztán ki lehet egészíteni később mind funkcionálisan, mind tartalommal. Célunk a dotnet-alapú multiplatform fejlesztések lehetőségeinek feltárása és a kiválasztott módszerekkel és eszközökkel a program elkészítése. Megvalósítandó platformok a következők:

*	Windows (desktop)
*	Android
*	Linux

A második félévben megvalósítandó funkciók:
*	többjátékos mód
*	egy központi szerver, ami küldi a kliensnek a játékkal kapcsolatos adatokat
*	online ranglisták
*	tutorial

### 1.2 Szakterületi fogalomjegyzék
- hidden object játék - olyan játék, amiben egy statikus háttér fölött, abba beleolvadó objektumokat kell megkeresni
- casual gamer -  az az átlagos mobiltelefon-használó, akit az egyszerű, rövid játékmenetű, figyelemelvonó játékok érdekelnek
- gameplay prototype - alap funkcionalitású játék, amin látható és tesztelhető a játékmenet maga, minden egyéb kiegészítés nélkül

### 1.3 Használatieset-modell, funkcionális követelmények

![Használati eset modell](/readme_resources/usecase1.jpg?raw=true "Használati eset modell")

A következő félévre vonatkozó kiegészített használatieset-modellünk:

![Használati eset modell 2](/readme_resources/usecase2.jpg?raw=true "Használati eset modell 2")

### 1.4 Szakterületi követelmények

*	pályák alakját template határozza meg
*	Véletlenszerű pálya a képekre vonatkozóan
*	Kép paraméterekkel rendelkezik a nehézségre vonatkozóan:
  *	Elforgatás
  *	Homályosítás
  *	Tükrözés
  *	Színezés
*	(Egész pályára vonatkozóan:
  *	Időlimit
  *	Méretlimit)
*	Játékmódok:
  *	Normál – fix számú kép megkeresése, idő alapján xp
  *	Idő extra – ha megvan egy kép, akkor az idődet kitolja x másodperccel (hibásnál levon időt)
  *	Végtelenített – minimális xp, de hatalmas pálya. Ha megtalálsz egyet, ahelyett új kép jelenik meg
*	Beállítások: hang, vibráció  
A következő félévre vonatkozó szakterületi követelményeink:
* Többszereplős játékmódok:
  * Időzítő – két személy játszik egymás ellen, külön kapnak fix időt. A játékosok felváltva játszanak, a felváltás egy kép megtalálása után történik. Az veszít, akinek előbb letelik az ideje
  * Harc a pontokért – két személy játszik egymás ellen. A játékosok egyszerre játszanak, egy fix feladványt oldanak meg. Az nyer, akinek több pontja lesz a feladvány befejezésekor.
* A transzformációk átkerülnek a szerverre
* Ranglista
* Tutorial – Egy példajátékon vezet végig.
* A szerver szolgáltatásai:
  * Scoreboard elküldése a kliensnek
  * Egyjátékos-mód
    * Indításkor pálya generálása
    * Endless módhoz új pályaelem
    * Scoreboard frissítése
  * Többjátékos-mód
    * Játékosok regisztrálása, párosítása
      * indításkor pálya generálása, küldése
      * állapot frissítése, szinkronizálása


### 1.5 Nem funkcionális követelmények

#### Fejlesztési módszertan:
Egységesített Eljárás

#### A fejlesztéshez szükséges hardver:
Windows 7 vagy újabb operációs rendszer futtatására alkalmas számítógép

#### A fejlesztéshez használt szoftverek:

##### Operációs rendszer:
Windows 7 vagy újabb
##### Fejlesztőkörnyezet:
Visual Studio 2012, 2013 vagy Xamarin Studio
##### Követelmény elemzés:
Google Docs szerkesztővel
##### CASE eszköz:
Enterprise Architect 8
##### A futtatáshoz szükséges operációs rendszer:
Teszteléshez Windows 7 vagy újabb
Kiadásnál Android 2.3 vagy újabb, iOS 6 vagy újabb, Windows Phone 8 vagy újabb
##### A futtatáshoz szükséges hardver:
Operációs rendszerek szerint megadva
##### Egyéb követelmények:
Intuitív felhasználói felület, könnyű kezelhetőség

### 1.6 Multiplatform fejlesztés eszközei

Mivel a project célja hogy Windows és több mobil platformon fusson a program, több keretrendszert megvizsgáltunk ami ezt lehetővé teszi C# segítségével. A fő szempontok a kényelem, kódmegosztás lehetősége és kezelőfelület készítésének egyszerűsége voltak.

#### 1.6.1 Xamarin

A Xamarin egy cross-platform mobilfejlesztésre használható keretrendszer. Segítségével
képesekek lehetünk Android, iOS és Windows Phone alkalmazások készítésére, anélkül, hogy mindegyik platformra elölről kellene kezdeni a fejlesztést.
A fejlesztést C# nyelvben tudjuk végezni, a .NET keretrendszerre támaszkodva.

##### A platformok közötti kódmegosztásnak három módja van:

###### 1) Megosztott projektek
Ebben az esetben minden platform külön projektet igényel és a közös kód a 
megosztott projektben szerepel.

Előnyei:
Több projekt közötti kódmegosztás. Frissítéskor az összes frissül. A közös kódban 
platformfüggő elágazások is szerepelhetnek, pl. #if_ANDROID_


Hátrányai:
A megosztott projektnek nincsen output assemblyje, tehát ha a fordított kódot DLL-ként szeretnénk megosztani, akkor PCL-t kell használni.

###### 2) PCL

Előnyei:
Kódmegosztás. Ha a központi kódban refactorálunk, az kihat a platformfüggő projektekre is.
Egy PCL projektre egyszerűen lehet hivatkozni más projektekből a solutionon belül, illetve az output assemblyt meg lehet osztani a többi projekt között.

Hátrányai:
Mivel ugyanaz a PCL van megosztva több platform között, platformspecifikus könyvtárakat nem tudunk meghívni (pl. CommunityCsharpSqlite.WP7).

A PCL esetleg nem tartalmaz olyan osztályokat amelyek elérhetők mind Mono és MonoTouch-ban Androidhoz (pl DllImport és System.IO.File).

###### 3) Xamarin.Forms

Xamarin.Forms APIkat használva egyszerűen lehet natív cross-platform alkalmazásokat készíteni, ráadásul az interface kódja lehet akár 100%-ban közös.

Runtime alatt az összes interface rendre megjelenik, úgy, ahogy a közös C# kódban azt megírtuk. Persze az objektumuk alkalmazkodnak a megfelelő platformhoz. Pl. egy Xamarin.Forms Entry iOS alatt UITextView, Andoid alatt EditText, WP alatt TextBoxszá 
alakul.
Minden egyes layout esetén eldönthetjük, hogy Xamarin.Formsot, vagy platformfüggő megoldásokat választunk.
Az applikációt a megszokott és újrahasznosíthatósága miatt nagyon hatékony MVVM architektúrában írhatjuk meg, DataBindingot, és a layout esetén XAML kódot használva.
A Xamarin.Forms rengeteg beépített elemet (animációk, layout sablonok, gombok...) tartalmaz, és a Xamarin saját IDE-jéből, valamint Visual Studio alatt is használható (megfelelő csomag vásárlása esetén.)

##### Árazás

A Xamarin csak bizonyos esetben ingyenes. Magánszemélyek számára egy limitált, ingyenes kiadást biztosítanak, melyben a Xamarin.Forms és a bármekkora app méret nem szerepel.
Az Indie verzióban a Xamarin.Forms-t már garantálják, de a Visual Studioba való beépíthetőség még mindig nincs biztosítva. Ez utóbbi nem akkora hátrány, hiszen a Xamarin Studio IDE minden csomagnak része. Ezzel az opcióval bármekkora appot fejleszthetünk.
Mindez havi $25 ~ 6187 HUF-ba kerül, és ez az utolsó csomag, melyet természetes személyek igényelhetnek.

#### 1.6.2 MonoGame
A MonoGame a Microsoft által készített, mára már elavulttá vált XNA keretrendszer nyílt forrású újraimplementált változata. Nagyon népszerű keresztplatformos alkalmazások (főleg játékok) készítésénél, mivel a keretrendszer szinte minden létező platformon alkalmazható valamilyen módon: Xbox 360 (XNA), Windows és Windows Phone (XNA), iOS (MonoGame + Xamarin), Android (MonoGame + Xamarin), Mac OS X (MonoGame), Linux (MonoGame), Windows 8 Metro (XNA), PlayStation Mobile (MonoGame, részlegesen), Raspberry PI (MonoGame, részlegesen) és PlayStation 4 (MonoGame, részlegesen).

##### Felépítés
Viszonylag alacsonyszintű, hardver-közeli megoldásokat ad az ember kezébe, amivel hatékonyan ki tudja használni az eszközök grafikai- és processzor-teljesítményét. Azonban nyújt ezekre épülő magasszintű rendszereket is, melyek felgyorsítják és kényelmessé teszik a fejlesztést. Ezek olyan szolgáltatások amikre szinte minden interaktív alkalmazásnak szüksége van, pl. kötegelt sprite-renderelés, multimédiás formátumok betöltése, stb.

[A MonoGame felépítése](http://www.digitalrune.com/Support/Blog/tabid/719/EntryId/49/XNA-4-0-Class-Diagrams.aspx)

##### Kezelőfelület létrehozása
GUI kezelést beépítve nem tartalmaz ám ehhez a projecthez ez nem lényeges, mivel kódból egyszerűen kirajzolhatóak a grafikai elemek és kezelhetőek a bemeneti események.
##### Platformfüggetlen fejlesztés
A platformfüggetlen fejlesztés nem tökéletesen megoldható vele. A platformok között közös library-t nem lehet létrehozni, a kódmegosztás csak file-ok linkelésével vagy (automatizált) újbóli beöltésével oldható meg a projectek közt. Ugyanez a helyzet a program tartalmi részével (képek, hangok, stb. elemek).

Nagy hátrány, hogy Android és iOS platformokra mono fordító kell hozzá, és csak a Xamarin megfelelő. A MonoGame méretéből adódóan az ingyenes változattal nem is működik együtt.

Ehhez alternatíva lehet a **dot42** nevű fordító ami dotnet bytecode-ból Dalvik (Android virtuális gép) bytecode-ot fordít, ám ez MonoGame-mel alapvetően nem kompatibilis és csak magáncélú felhasználásra ingyenes.

#### 1.6.3 MonoCross
A Monocross egy több projektet összefogó keretrendszer. A Mono projekt különböző platformjait fogja össze (MonoTouch – iOS, MonoDroid – Android), amik C# .NET alapokon való fejlesztést tesznek lehetővé. Ezáltal egy keretrendszer alatt lehet minden platformra kódot írni.
A MonoCross kódmegosztáson alapszik, az MVC pattern-t alkalmazza. Minden platform egy különböző Container-be került, ami a C#-ban található projekttel egyenértékű. A nem megosztott kódban van lehetőség natív API hívásokra, így kihasználhatóak az adott eszköz egyedi funkciói.

A működési elve a következő:

##### Model:
Első lépésként meg kell tervezni és valósítani a Modellt. Ez teljes egészében közös kód.

##### Controller:

Minden modell típushoz külön controller kell, MXController<T>-ből leszármaztatva. Ez inicializálja a modelleket és köti össze a hozzá tartozó nézettel.
Egy úgynevezett Navigációs térkép a nézetek közötti folyamatok alapja. A nézet a mobileszközön egy megjelenített oldal, ami definiálható néhány megadott tulajdonsággal.
Ezen a NavigationMap-en kell regisztrálni a nézeteket, URI formában megadott tulajdonságokkal. Pl.: "{Kategoria}/{Termek}"

Mivel a nézetek ilyen pontosan definiálhatóak, nagyon könnyű navigálni közöttük.
Ezek a kötések szintén közösek a Containerek között.

Megvalósítandó függvény:
*	Load() – A Controller betöltésekor fut le, itt kell elvégezni a modellek inicializálását és a kötéseket.

##### View:
A View-ban definiáljuk a nézetek kinézetét. Itt már lehet platformspecifikus tulajdonságokat használni. IMXView interfészt implementálni kell, lehet MXView<T>-ből is.

Megvalósítandó függvény:
*	Render() - kirajzolás

##### App:
Ezután már csak inicializálni kell az Appot, és hozzákötni egy Containerhez, és deploy után futtatható.

##### A keretrendszer előnyei:
*	Könnyen átlátható
*	Maga a Monocross ingyenes
*	A NavigationMap nagyon pontos navigációt tesz lehetővé

##### Hátrányai:
*	Mivel a Mono-t más keretrendszerek is használják, ezért nem sok pluszt nyújt.
*	Nagyon sok lehet a platformspecifikus kód, így nagy programok esetében kényelmetlen lehet

#### 1.6.4 A választott rendszer
Végül a MonoGame keretrendszerre esett a választás. Ennek főbb okai:
*	sok támogatott platform, ám alacsonyszintű rendszerek is elérhetőek belőle
*	Windows alatt fejlesztéshez és teszteléshez elég az ingyenes Visual Studio hozzá
*	felépítése jól illeszkedik a játékok működéséhez (hiszen ahhoz tervezték) és ez jelentősen megkönnyíti a program fejlesztését

### 1.7 User-story-k

Minden használati eset tartalmaz egy Határidő sort, ez tartalmazza, hogy az adott esetnek melyik félévben kell elkészülnie. A 2014 az 1. félévet jelenti, a 2015 a 2. félévet.

| Használati eset                    | Menü           |
| ---------------------------------- | -------------- |
| Cél                                | A játékos kiválasztja, hogy melyik főpontot választja. Ezek lehetnek az Egyszemélyes játék, a Többszemélyes játék, a Toplista megtekintése, illetve a Tutorial elindítása |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A játék elindítása |
| Utófeltétel sikeres végrehajtáskor | A kiválasztott menüponthoz kapcsolódó felhasználói eset következik: Egyjátékos/Többjátékos mód beállítása, Toplista, Tutorial  |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe |
| Határidő                           | 2015 |

| Használati eset                    | Toplista       |
| ---------------------------------- | -------------- |
| Cél                                | A játékban elért pontszámok megtekintése, választhatóan játékmód és nehézség szerint. Az első helyen a legtöbb pontot elért játékos és a neve található. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A Scoreboard gomb megnyomása |
| Előfeltételek                      | A szerver elérhető |
| Utófeltétel sikeres végrehajtáskor | Megjelenik a Toplista |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe. |
| Határidő                           | 2015 |

| Használati eset                    | Tutorial       |
| ---------------------------------- | -------------- |
| Cél                                | A felhasználó végigvezetése lépésről-lépésre egy egyszemélyes játékmódon, hogy megtanulhassa a szoftver kezelését. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A menüben a Tutorial elindítása került kiválasztásra. |
| Előfeltételek                      | Az egyjátékos mód elindítható (szerver elérhető). |
| Utófeltétel sikeres végrehajtáskor | Elindul egy játék, amiben a felhasználót minden cselekvése előtt szövegdobozok segítik a szoftver kezelésében és a játék végigjátszásában. |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe. |
| Határidő                           | 2015 |

| Használati eset                    | Egyjátékos/Többjátékos mód beállítása       |
| ---------------------------------- | -------------- |
| Cél                                | A játék tulajdonságainak beállítása. A tulajdonságok közé a következők tartoznak: Játékmód, Nehézség |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A Single Player vagy a Multiplayer gomb megnyomása |
| Előfeltételek                      | A menüből ki lett választva a megfelelő gomb |
| Utófeltétel sikeres végrehajtáskor | Elindul a várakozás a képek leöltésére. Többjátékos mód esetén Név megadása |
| Utófeltétel hiba esetén            | Nem indul el a játék, maradunk a beállításoknál. |
| Határidő                           | Egyjátékos mód: 2014, Többjátékos mód: 2015 |

| Használati eset                    | Név megadása       |
| ---------------------------------- | -------------- |
| Cél                                | A játékos nevének megadása |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | Multiplayer beállítás után a Játék gomb megnyomása |
| Előfeltételek                      | A többjátékos módban be lett állítva játékmód és nehézség |
| Utófeltétel sikeres végrehajtáskor | Elindul a várakozás a képek leöltésére |
| Utófeltétel hiba esetén            | Nem indul el a játék, maradunk a név megadásánál. |
| Határidő                           | 2015 |

| Használati eset                    | Normál játék   |
| ---------------------------------- | -------------- |
| Cél                                | A játék indítása után fix számú képet kell megkeresni. A pontozás idő alapján történik. A gyorsabb találat szorzót eredményez. A helyes találat pontot ér. A helytelen találat pontlevonással jár. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A játék gomb megnyomása. |
| Előfeltételek                      | Egyszemélyes játékmód választása. Név megadása. Nehézség kiválasztása. A Normál játék opció kiválasztása. |
| Utófeltétel sikeres végrehajtáskor | Elindul a ’Normál’ típusú játék. |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe. |
| Határidő                           | 2014 |

| Használati eset                    | Egy feladvány megtalálása ’Normál’ módban |
| ---------------------------------- | -------------------- |
| Cél                                | A felhasználó az alsó sávban található feladványok valamelyikét ábrázoló képre kattint, hogy pontokat szerezhessen. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A felhasználó egy képre kattint. |
| Előfeltételek                      | ’Normál’ játék folyamatban van, a kép egy megtalálandó feladványt ábrázol és aktív. |
| Utófeltétel sikeres végrehajtáskor | A felhasználó a kombó állása alapján pontokat kap (pl. 16-os kombó esetén 16 pontot). A kombó értéke duplázódik, ha nem több, mint 32. A kép inaktívvá válik. Az utolsó feladvány megtalálása esetén az eredmény megjelenítése. |
| Határidő                           | 2014 |

| Használati eset                    | Idő extra játék |
| ---------------------------------- | --------------- |
| Cél                                | A játék indítása után adott számú képet kell megtalálni. Minden helyes találat növeli az egyébként folyamatosan csökkenő rendelkezésre álló időt. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A játék gomb megnyomása. |
| Előfeltételek                      | Egyszemélyes játékmód választása. Név megadása. Nehézség kiválasztása. Az Idő extra játék opció kiválasztása. |
| Utófeltétel sikeres végrehajtáskor | Elindul az ’Idő extra’ típusú játék. |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe. |
| Határidő                           | 2014 |

| Használati eset                    | Egy feladvány megtalálása ’Idő extra’ módban |
| ---------------------------------- | -------------------- |
| Cél                                | A felhasználó az alsó sávban található feladványok valamelyikét ábrázoló képre kattint, hogy pontokat szerezhessen. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A felhasználó egy képre kattint. |
| Előfeltételek                      | ’Idő extra’ játék folyamatban van, a kép egy megtalálandó feladványt ábrázol és aktív. |
| Utófeltétel sikeres végrehajtáskor | A felhasználó a kombó állása alapján pontokat kap. A kombó értéke duplázódik, ha nem több, mint 32. A kép inaktívvá válik. Az utolsó feladvány megtalálása esetén az eredmény megjelenítése. |
| Határidő                           | 2014 |

| Használati eset                    | Végtelenített játék  |
| ---------------------------------- | -------------------- |
| Cél                                | A játék indítása után potenciálisan végtelen ideig lehet játszani: minden megtalált kép egy új feladatot eredményez, valamint a hozzá tartozó új képet a régi helyén. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A játék gomb megnyomása. |
| Előfeltételek                      | Egyszemélyes játékmód választása. Nehézség kiválasztása. A Végtelenített játék opció kiválasztása. |
| Utófeltétel sikeres végrehajtáskor | Elindul a ’Végtelenített’ típusú játék. |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe. |
| Határidő                           | 2014 |

| Használati eset                    | Egy feladvány megtalálása ’Végtelenített’ módban |
| ---------------------------------- | -------------------- |
| Cél                                | A felhasználó az alsó sávban található feladványok valamelyikét ábrázoló képre kattint, hogy pontokat szerezhessen. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A felhasználó egy képre kattint. |
| Előfeltételek                      | ’Végtelenített’ játék folyamatban van, a kép egy megtalálandó feladványt ábrázol. |
| Utófeltétel sikeres végrehajtáskor | A felhasználó a kombó állása alapján pontokat kap. A kombó értéke duplázódik, ha nem több, mint 32. A kliens a kép helyére új, transzformált képet kér a szerverről, valamint generál egy új feladványt. |
| Utófeltétel hiba esetén            | Ha a szerver nem elérhető, visszatérés a főmenübe. |
| Határidő                           | 2014 |

| Használati eset                    | Kilépés játékból |
| ---------------------------------- | -------------------- |
| Cél                                | A felhasználó egy játék játszása közben a ’Menü’ gombra kattintva kilép a főmenübe. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A ’Menü’ gomb megnyomása. |
| Előfeltételek                      | Egy játék folyamatban van. |
| Utófeltétel sikeres végrehajtáskor | A játék véget ér. Egyjátékos módban a kapott pontok nem kerülnek fel a scoreboardra; többjátékos mód esetén az ellenfél nyer. Visszatérés a főmenübe. |
| Határidő                           | 2014 |

| Használati eset                    | Eltelik egy másodperc |
| ---------------------------------- | -------------------- |
| Cél                                | Egyjátékos módban, vagy többjátékos módban, amikor a felhasználó következik, eltelik egy újabb másodperc. A kliens lépteti az idő számlálót. |
| Aktor                              | Kliens szoftver |
| Kiváltó esemény                    | Minden eltelt másodperc a játék indítása óta, amikor a játékos következik. |
| Előfeltételek                      | Egy játék folyamatban van, a felhasználó következik. |
| Utófeltétel sikeres végrehajtáskor | Az idő számláló minden másodpercben léptetésre kerül, ’Idő extra’ és ’Időzítő’ módokban visszafelé. Ha eléri a 0 hátralévő másodpercet: a játék véget ér, és megjelenik az eredmény. |
| Határidő                           | Idő extra mód: 2014, Időzítő: 2015 |

| Használati eset                    | A felhasználó játék közben inaktív |
| ---------------------------------- | -------------------- |
| Cél                                | A felhasználó egyjátékos vagy ’Harc a pontokért’ módban inaktív. A kliens felezi a kombót. |
| Aktor                              | Kliens szoftver |
| Kiváltó esemény                    | Minden eltelt 2500 ms az előző feladvány megtalálása óta. |
| Előfeltételek                      | Egyjátékos vagy ’Harc a pontokért’ játék folyamatban van, a felhasználó inaktív. |
| Utófeltétel sikeres végrehajtáskor | A kombó feleződik, ha nagyobb, mint egy, és az előző feladvány megoldása, vagy az előző felezés óta eltelt 2500 ms. |
| Határidő                           | 2015 |

| Használati eset                    | Egyjátékos módú eredmény megjelenítése |
| ---------------------------------- | -------------------- |
| Cél                                | Egy egyjátékos módú játék végeztével megjelenik a felhasználó által összegyűjtött pontok száma, valamint az eltelt, vagy megmaradt idő. Frissül a scoreboard. |
| Aktor                              | Felhasználó vagy kliens szoftver. |
| Kiváltó esemény                    | A felhasználó megtalálta az utolsó feladványt, vagy ’Idő extra’ módban lejárt az ideje. |
| Előfeltételek                      | - |
| Utófeltétel sikeres végrehajtáskor | Megjelenik a felhasználó által összegyűjtött pontok száma, valamint az eltelt, vagy megmaradt idő. A kliens REST API-n frissíti a scoreboardot. |
| Utófeltétel hiba esetén            | Ha a szerver nem elérhető, a scoreboard nem frissül. |
| Határidő                           | 2015 |

| Használati eset                    | Várakozás ellenfélre és a képek letöltésére       |
| ---------------------------------- | ------------------------------------------------- |
| Cél                                | A felhasználó várakozik, hogy a szerver megfelelő ellenfelet biztosítson, valamint letöltse a transzformált képeket és kezdődhessen a többjátékos mód.  |
| Aktor                              | Felhasználó. Internetes felhasználó. |
| Kiváltó esemény                    | Többjátékos módban a játék indítása. |
| Előfeltételek                      | Többjátékos mód választása. Név megadása. Nehézség kiválasztása. Játékmód választása. |
| Utófeltétel sikeres végrehajtáskor | A szerver párosít két játékost. |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe. |
| Határidő                           | 2015 |

| Használati eset                    | Várakozás a képek letöltésére       |
| ---------------------------------- | ----------------------------------- |
| Cél                                | A felhasználó várja, hogy a klienshez megérkezzen a REST API-n kérvényezett játék, miközben a szerver legenerál egy pályát a választott játékmód, szint és téma szerint (ehhez képeket választ és transzformál véletlenszerűen), és küldi a kliensnek. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A felhasználó kezdeményezte egy játék elindítását. |
| Előfeltételek                      | A felhasználó megadta a pálya beállításait, a szerver elérhető. |
| Utófeltétel sikeres végrehajtáskor | Elindul a kért játék. |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe. |
| Határidő                           | 2015 |

| Használati eset                    | Időzítő       |
| ---------------------------------- | ------------- |
| Cél                                | Két személy játszik egymás ellen, külön kapnak fix időt. A játékosok felváltva játszanak, a váltás egy kép megtalálása után történik. Az veszít, akinek előbb letelik az ideje. |
| Aktor                              | Felhasználó. Internetes felhasználó. |
| Kiváltó esemény                    | A szerver sikeresen párosított két játékost. |
| Előfeltételek                      | Elérhető két játékos ’Időzítő’ módban, azonos nehézségi szinten. Nem lép fel hálózati hiba. |
| Utófeltétel sikeres végrehajtáskor | Elindul a kétszemélyes játék ’Időzítő’ módban. |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe. |
| Határidő                           | 2015 |

| Használati eset                    | Egy feladvány megtalálása ’Időzítő’ módban |
| ---------------------------------- | -------------------- |
| Cél                                | A felhasználó az alsó sávban található feladványok valamelyikét ábrázoló képre kattint, hogy több ideje maradjon, mint ellenfelének. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A felhasználó egy képre kattint. |
| Előfeltételek                      | ’Időzítő’ játék folyamatban van, a felhasználó következik, a kép egy megtalálandó feladványt ábrázol. |
| Utófeltétel sikeres végrehajtáskor | A kliens leállítja az órát, és TCP/IP socketen jelzi a szervernek a feladvány megoldását. A szerver a kép helyére új, transzformált képet, valamint új feladványt generál, és szinkronizálja az új játékállapotot a kliensekkel. |
| Utófeltétel hiba esetén            | Hálózati hiba esetén visszatérés a főmenübe. |
| Határidő                           | 2015 |

| Használati eset                    | Várakozás ellenfélre ’Időzítő’ módban |
| ---------------------------------- | ------------------------- |
| Cél                                | A felhasználó ellenfele lépésére várakozik. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | ’Időzítő’ módban az ellenfél lépése következik. |
| Előfeltételek                      | ’Időzítő’ játék folyamatban van, az ellenfél következik.  |
| Utófeltétel sikeres végrehajtáskor | Az ellenfél lépése, majd a játékállapot szinkronizálása után a felhasználó következik. Az óra újraindul. |
| Utófeltétel hiba esetén            | Hálózati hiba esetén visszatérés a főmenübe.  |
| Határidő                           | 2015 |

| Használati eset                    | Harc a pontokért       |
| ---------------------------------- | ---------------------- |
| Cél                                | Két személy játszik egymás ellen. A játékosok egyszerre játszanak, egy fix feladványt oldanak meg. Az nyer, akinek több pontja lesz a feladvány befejezésekor. |
| Aktor                              | Felhasználó. Internetes felhasználó. |
| Kiváltó esemény                    | A szerver sikeresen párosított két játékost. |
| Előfeltételek                      | Elérhető két játékos ’Harc a pontokért’ módban, azonos nehézségi szinten. Nem lép fel hálózati hiba. |
| Utófeltétel sikeres végrehajtáskor | Elindul a kétszemélyes játék ’Harc a pontokért’ módban. |
| Utófeltétel hiba esetén            | Visszatérés a főmenübe. |
| Határidő                           | 2015 |

| Használati eset                    | Egy feladvány megtalálása ’Harc a pontokért’ módban |
| ---------------------------------- | -------------------- |
| Cél                                | A felhasználó az alsó sávban található feladványok valamelyikét ábrázoló képre kattint, hogy több pontot szerezzen, mint ellenfele. |
| Aktor                              | Felhasználó |
| Kiváltó esemény                    | A felhasználó egy képre kattint. |
| Előfeltételek                      | ’Harc a pontokért’ játék folyamatban van, a kép egy megtalálandó feladványt ábrázol és aktív. |
| Utófeltétel sikeres végrehajtáskor | A felhasználó a kombó állása alapján pontokat kap. A kombó értéke duplázódik, ha nem több, mint 32. A kép inaktívvá válik. Játékállapot szinkronizálása a szerverrel. Az utolsó feladvány megtalálása esetén az eredmény megjelenítése. |
| Utófeltétel hiba esetén            | Hálózati hiba esetén visszatérés a főmenübe. |
| Határidő                           | 2015 |

| Használati eset                    | Többjátékos módú eredmény megjelenítése |
| ---------------------------------- | -------------------- |
| Cél                                | Egy többjátékos módú játék végeztével megjelenik a felhasználó által összegyűjtött pontok száma, vagy a megmaradt idő, valamint a nyertes játékos neve. |
| Aktor                              | Felhasználó, kliens szoftver vagy szerver. |
| Kiváltó esemény                    | A felhasználó megtalálta az utolsó feladványt, vagy ’Időzítő’ módban lejárt valamelyik játékos ideje. |
| Előfeltételek                      | A végső játékállapot szinkronizációja sikeres. |
| Utófeltétel sikeres végrehajtáskor | Megjelenik a felhasználó által összegyűjtött pontok száma, vagy a megmaradt idő, valamint a nyertes játékos neve. |
| Utófeltétel hiba esetén            | Hálózati hiba esetén visszatérés a főmenübe. |
| Határidő                           | 2015 |

### 1.8 Tervezett felosztás

A 2015-ös félévre a tervezett feladatok a következőképpen kerültek felosztásra:

Rabi Péter:
* Szerver adatbázis
* Szerverrel való kommunikáció REST API-n keresztül
* Szerverrel való kommunikáció a multiplayerhez Socketeken keresztül
* A szervertől való kérések megfelelő kiszolgálása
* Képek transzformációja a szerveren

Birkás Gábor:
* Időzítő játékmód implementálása
* Harc a pontokért játékmód implementálása

Csatári Albert:
* Tutorial
* Scoreboard
* Kommunikáció a szerverrel REST API-n és Socketeken keresztül
* Egyjátékos mód frissítése szerverrel való kommunikációhoz
* Platformfüggetlen megjelenítés
* Menürendszer továbbfejlesztése az új követelményekhez

## 2.	Tervezés
### 2.1 A program architektúrája
A szoftver egy háromrétegű webes alkalmazás, natív klienssel.

Adatbázis szerver:
* Relációs adatbázis-kezelő rendszer.

Alkalmazás szerver:
* A web2py keretrendszerre épülő Python nyelven fejlesztett szoftver.
* Kapcsolódás a kliensekhez:
  * RESTful web-szolgáltatások
  * dedikált TCP/IP socket a többjátékos mód megvalósításához

A kliens program legfontosabb elemei:
* 2 fő mappa, ami a két különböző funkciójú tulajdonságait tartalmazza:
  * Jatek mappa - A játékkal kapcsolatos logika és nézet
  * Menu mappa - A menürendszerrel kapcsolatos logika és nézet
* A menü nézetei:
  * Menu absztrakt osztály, ami egy menü nézet alapvető feladatait látja el: Kirajzolás, Menü elemek kezelése, Kattintás vagy érintés kezelése
  * MainMenu: A kezdő menü
  * NewGameMenu: Az egyjátékos illetve a többjátékos mód beállításait tartalmazza
  * ScoreboardMenu: A ranglista menüje
  * GameOverMenu: A játék befejezése után megjelenített menü. A szerzett pontot mutatja és a pontot lehet elküldeni
* A kliens architektúrája úgy lett kialakítva, hogy a MonoGame keretrendszer által támogatott legyen:
Van egy Common nevű mappa, ahol találhatóak a forráskódok. Minden platform külön projektet kap, és ide a Common mappában lévő forráskódok csak linkelve vannak.
A platformspecifikus kódok vagy teljesen a projektben találhatóak (nincsenek linkelve), vagy #if ANDROID (#if WINDOWS) direktívákkal vannak ellátva.
A MonoGame egyetlen sajátossága, hogy az XNA-hoz hasonlóan négy eljárást kell megvalósítania egy nézetnek:
  * Initialize
  * LoadContent
  * Update
  * Draw

### 2.2 Osztálymodell

![Osztálymodell](/readme_resources/objectmodell1.jpg?raw=true "Osztálymodell")

##### Játék
**Sztereotípia:** Vegyes  
**Példány:** jatekModell  
**Feladat:** Egy játékkal kapcsolatos minden tulajdonságot tartalmaz.  
##### Pontozó
**Sztereotípia:** egyed  
**Példány:** pontozó  
**Feladat:** Kezeli a pontok számolását a kombókkal együtt. Ha a ponthoz hozzáadás történik, a kombó a kétszeresére nő. A hozzáadott pont a kombóval szorzódik. A kombó 32-ig képes nőni, és ha nem történik ponthozzáadás 2,5 másodpercen belül, a kombó feleződik.  
##### Pályamenedzser
**Sztereotípia:** egyed  
**Példány:** pálya  
**Feladat:** Kezeli az új játék kezdését, illetve az egyéb egy játékmóddal kapcsolatos tulajdonságokat  
##### Tábla
**Sztereotípia:** konténer  
**Példány:** tabla  
**Feladat:** Képes létrehozni egy több mezőből álló pályát, és azt tárolja is utána. Pontos meghatározás után (sor-oszlop indexelés) vissza is adja a mezőt.  
##### Mező
**Sztereotípia:** egyed  
**Példány:** összesMező, listában tárolva  
**Feladat:** meghatározza a játéktéren található mezőt. A tulajdonságai között szerepel a kép, az indexek, illetve egy transzformátor  
##### Kép
**Sztereotípia:** egyed  
**Példány:** kép  
**Feladat:** Tárol egy képet egy szöveggel együtt.  
##### KépAdatbázis
**Sztereotípia:** konténer  
**Példány:**  
**Feladat:** le tudja tölteni az összes képet, és későbbre tárolja is  
##### KépLetöltő
**Sztereotípia:** interfész  
**Példány:** letöltő  
**Feladat:** Egy interfész, ami le tudja tölteni az összes képet platformfüggetlenül.  
##### Transzformáció
**Sztereotípia:** interfész  
**Példány:** transzformátor  
**Feladat:** Nehézség alapján transzformál egy képet: forgatja, homályosítja, tükrözi vagy elszínezi  
##### Mentés
**Sztereotípia:** absztrakt osztály  
**Példány:** mentésMenedzser  
**Feladat:** El tud menteni egy játékállást, illetve be is tudja tölteni platformfüggetlenül. A szerializálást is megoldja (ezt nem szükséges minden platformon külön megvalósítani).  
##### BeállításokMenü
**Sztereotípia:** vegyes  
**Példány:** menuModell  
**Feladat:** A beállítások menüjének modellje  
##### Beállítások
**Sztereotípia:** egyed  
**Példány:** beállítások  
**Feladat:** Az összes beállítást tartalmazza  
##### HangÉsVibráció
**Sztereotípia:** interfész  
**Feladat:** Platformfüggetlenül valósítja meg hang lejátszását és a mobileszköz rezgését. 

Az osztálymodell kiegészítése a következő követelményekkel:
* Tutorial, Scoreboard
* Kommunikáció a szerverrel

Terv:  
![Osztálymodell 2](/readme_resources/objectmodell2_albert.png?raw=true "Osztálymodell kiegészítés")

##### SzerverKommunikátor  
**Sztereotípia:** absztrakt osztály  
**Példány:** szerverKommunikator  
**Feladat:** Egy általános felületet ad a szerverrel való kommunikációra  
##### ToplistaKommunikátor  
**Sztereotípia:** Egyed  
**Példány:** toplistaKommunikator  
**Feladat:** lekérdezi a szervertől az első 10 játékost a toplistán az adott játékmódból  
##### ToplistaRajzoló  
**Sztereotípia:** egyed  
**Példány:** toplistaRajzolo  
**Feladat:** Kirajzolja a toplistát a toplistakommunikátor alapján  
##### Tutorial  
**Sztereotípia:** Egyed  
**Példány:** tutorial  
**Feladat:** Elindítja a tutorialt.  
##### Menu  
**Sztereotípia:** egyed  
**Példány:** menu  
**Feladat:** ez az osztály felel a Menü kirajzolásáért, itt lehet kiválasztani a játékmódot, illetve a témát  

Az osztálymodell kiegészítése a következő követelményekkel:
* Sakk stílusú időosztásos játékmód
* Harc a pontokért játékmód

![Osztálymodell 3](/readme_resources/Multiplayer.jpg?raw=true "Osztálymodell kiegészítés multiplayerrel")

##### Játékmód  
**Sztereotípia:** enumerátor osztály  
**Példány:** játékMód  
**Feladat:** A meglévő Játékmód osztály kiegészítése többjátékos módokkal 
##### JátékosInfoMenü  
**Sztereotípia:** egyed  
**Példány:** játékosInfoMenü  
**Feladat:** Egy menü létrehozása, melyben a felhasználó megadja az adatait
##### JátékmódMenü  
**Sztereotípia:** egyed  
**Példány:** játékmódMenü  
**Feladat:** Egy menü létrehozása, melyben a felhasználó kiválasztja a játékmódot
##### PárosítóMenü  
**Sztereotípia:** egyed  
**Példány:** párosítóMenü  
**Feladat:** Egy menü létrehozása, melyben megjelenik a szerver által szolgáltatott ellenfél
##### MultiMódPontozó  
**Sztereotípia:** egyed  
**Példány:** multiMódPontozó  
**Feladat:** Tárolja az aktuális játékhoz tartozó állást (pontok, idők), valamint a játék menetének megfelelően a játékmódtól függően kiszámolja és frissíti azt
##### Tábla  
**Sztereotípia:** konténer   
**Példány:** tábla  
**Feladat:** A Tábla osztály kiegészítése többjátékos módhoz tartozó információkkal (játékosok, pontok, idők)


Végleges osztálymodell névterekre lebontva:

* Pixeek:

![Pixeek](/readme_resources/classdiagrams/pixeek.png?raw=true "")

##### Scene  
**Sztereotípia:** interfész   
**Példány:** currentScene  
**Feladat:** Egy nézet megvalósításához szükséges  

##### GameManager  
**Sztereotípia:** egyed   
**Példány:** Instance  
**Feladat:** A játékot indítja el és állítja be az alapvető beállításokat  

* Pixeek.BoardShapes:

![Pixeek.BoardShapes](/readme_resources/classdiagrams/pixeek-boardshapes.png?raw=true "")

##### IBoardShapes  
**Sztereotípia:** interfész   
**Példány:** boardShape  
**Feladat:** Egy alakzat leírását definiálja  

##### BoardRectangle  
**Sztereotípia:** egyed   
**Példány:** -  
**Feladat:** A négyzet alakú alakzatot reprezentálja  

##### BoardDiamond  
**Sztereotípia:** egyed   
**Példány:** -  
**Feladat:** A gyémánt alakú alakzatot reprezentálja  

##### BoardFish  
**Sztereotípia:** egyed   
**Példány:** -  
**Feladat:** A halacska alakú alakzatot reprezentálja  

* Pixeek.Game:

![Pixeek.Game](/readme_resources/classdiagrams/pixeek-game.png?raw=true "")

##### Difficulty  
**Sztereotípia:** enum osztály   
**Példány:** difficulty  
**Feladat:** A játékban található nehézségek felsorolója  

##### Image
**Sztereotípia:** egyed  
**Példány:** image  
**Feladat:** Tárol egy képet egy szöveggel együtt.  

##### Pályamenedzser
**Sztereotípia:** egyed  
**Példány:** level  
**Feladat:** Kezeli az új játék kezdését, illetve az egyéb egy játékmóddal kapcsolatos tulajdonságokat  

##### Board
**Sztereotípia:** konténer  
**Példány:** board  
**Feladat:** Képes létrehozni egy több mezőből álló pályát, és azt tárolja is utána. Pontos meghatározás után (sor-oszlop indexelés) vissza is adja a mezőt.  

##### Field
**Sztereotípia:** egyed  
**Példány:** allFields, listában  
**Feladat:** meghatározza a játéktéren található mezőt. A tulajdonságai között szerepel a kép, az indexek

##### ImagesToFind
**Sztereotípia:** konténer  
**Példány:** imagesToFind 
**Feladat:** Tárolja a feladvánnyal kapcsolatos információkat: hány kép van még hátra, a képek listája  

##### Scoring
**Sztereotípia:** egyed  
**Példány:** scoring  
**Feladat:** Kezeli a pontok számolását a kombókkal együtt. Ha a ponthoz hozzáadás történik, a kombó a kétszeresére nő. A hozzáadott pont a kombóval szorzódik. A kombó 32-ig képes nőni, és ha nem történik ponthozzáadás 2,5 másodpercen belül, a kombó feleződik.   

##### GameMode
**Sztereotípia:** enum osztály  
**Példány:** gameMode  
**Feladat:** Tárolja a lehetséges játékmódokat  

##### GameModel
**Sztereotípia:** vegyes   
**Példány:** gameModel  
**Feladat:** A játékok logikájával és nézetével kapcsolatos információkat tartalmazza  

* Pixeek.GameDrawables:

![Pixeek.GameDrawables](/readme_resources/classdiagrams/pixeek-gamedrawables.png?raw=true "")

##### RectangleOverlay
**Sztereotípia:** egyed  
**Példány:** overlay  
**Feladat:** Egy képpel ellátott négyzet kirajzolását valósítja meg  

##### TimeDrawable
**Sztereotípia:** egyed  
**Példány:** timeDrawable  
**Feladat:** A számláló kirajzolásához szükséges információkat tartalmazza. Ki is rajzolja az időzítőt.  

##### ScoreDrawable
**Sztereotípia:** egyed  
**Példány:** scoreDrawable  
**Feladat:** A pontok kirajzolásáért felelős (ehhez jár a kombók kirajzolása is  

##### ToFindDrawable
**Sztereotípia:** egyed  
**Példány:** toFindDrawable  
**Feladat:** A megkeresendő képek kirajzolásáért felelős  

##### UpperMenu
**Sztereotípia:** konténer  
**Példány:** Instance  
**Feladat:** A teljes felső menüt fogja össze és tárolja  

##### BoardDrawable
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** A tábla kirajzolásáért felelős, amin találhatóak a képek  


* Pixeek.Menus:

![Pixeek.Menus](/readme_resources/classdiagrams/pixeek-menus.png?raw=true "")

##### Menu
**Sztereotípia:** absztrakt osztály    
**Feladat:** Egy különálló menü megvalósításához szükséges függvényeket tartalmazza, mint a kirajzolás, a nézetváltás és az elemek kezelése  

##### GameOverMenu
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** A játék vége menüt valósítja meg  

##### ScoreboardMenu
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** A ranglistákat mutató menüt valósítja meg

##### MainMenu
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** A kezdőképernyőn megjelenő főmenüt valósítja meg    

##### NewGameMenu
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** Az új játékot tartalmazó beállításokat tartalmazó menüt valósítja meg  

* Pixeek.Menus.MenuElements:

![Pixeek.Menus.MenuElements](/readme_resources/classdiagrams/pixeek-menus-elements.png?raw=true "")

##### MenuElement
**Sztereotípia:** egyed  
**Példány:**   
**Feladat:** Egy menün található elemre vonatkozó információkat és függvényeket tartalmaz  

##### MenuSpriteElement
**Sztereotípia:** egyed  
**Példány:** menuSpriteElement  
**Feladat:** Egy képpel és szöveggel ellátott menüelem (legtöbbször egy kép vagy egy szöveg kirajzolására)  

##### TutorialButton
**Sztereotípia:** egyed  
**Példány:** tutorialButton  
**Feladat:** A TutorialElement kis négyzetén kívüli részek gombok, amik előidézik a tutorial folytatását. Ez valósítja meg ezeket a gombokat  

##### MenuSelector
**Sztereotípia:** egyed  
**Példány:**   
**Feladat:** Egy szótárban tartalmaz összetartozó menüelemeket, amik közül választani lehet  

##### MainMenuPlainSelector
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** Egy menüben megjeleníthető selectort valósít meg, aminek beállíthatóak a méretei, illetve egy enum, amiből válogat  

##### MenuTextElement
**Sztereotípia:** egyed  
**Példány:** menuTextElement  
**Feladat:** Megvalósít egy olyan elemet, ami szöveg befogadására alkalmas Windowson és Androidon is  

##### ScoreboardElements
**Sztereotípia:** egyed  
**Példány:** scoreboardElements  
**Feladat:** A selectorhoz hasonlóan felsorolja a kapott pontszámokat, beállítható méretekkel  

##### TutorialElement
**Sztereotípia:** egyed  
**Példány:** tutorialButton  
**Feladat:** Egy olyan tutorial elemet tartalmaz, ami a játéktér egy kis négyzetére fokuszál és hozzá egy szöveget mutat  

* Pixeek.ServerCommunicator:

![Pixeek.ServerCommunicator](/readme_resources/classdiagrams/pixeek-servercommunicator.png?raw=true "")

##### ServerCommunicator
**Sztereotípia:** absztrakt osztály    
**Feladat:** A szerverrel való kommunikációhoz ad függvényeket. Így egy egyszerű send vagy listen függvénnyel lehet kommunikálni REST API-val vagy Socketekkel

##### FightGameCommunicator
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** A Harc a pontokért játékhoz tartozó socketes kommunikációt valósítja meg  

##### TimerGameCommunicator
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** Az Időzítő játékhoz tartozó socketes kommunikációt valósítja meg  

##### MultiPlayerGameCommunicator
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** A multiplayer játékokhoz tartozó általánosabb hívásokat tartalmazza  

##### ScoreboardCommunicator
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** A ranglistákhoz tartozó Rest API hívásokat tartalmazza  

##### SinglePlayerGameCommunicator
**Sztereotípia:** egyed  
**Példány:** Instance  
**Feladat:** Az egyjátékos játékokhoz tartozó REST API hívásokat tartalmazza  

* Pixeek.ServerCommunicator.Objects:

![Pixeek.ServerCommunicator.Objects](/readme_resources/classdiagrams/pixeek-servercommunicator-objects.png?raw=true "")

Az ebben a névtérben szereplő osztályok a szerverhez való kommunikációt segítik. Strukturális segítséget nyújtanak az üzenetek JSON formájáról való átalakításra.  

* Pixeek.Settings:

![Pixeek.Settings](/readme_resources/classdiagrams/pixeek-settings.png?raw=true "")

##### Settings
**Sztereotípia:** egyed  
**Példány:** settings  
**Feladat:** Az aktuális beállításokat tartalmazza a vibrációra és a hangra vonatkozóan  

##### SettingsMenu
**Sztereotípia:** egyed  
**Példány:** settingsMenu  
**Feladat:** A beállításokat tartalmazó menü elem

* Pixeek.SoundVibration:

![Pixeek.SoundVibration](/readme_resources/classdiagrams/pixeek-soundandvibration.png?raw=true "")

##### SoundAndVibration
**Sztereotípia:** interfész    
**Feladat:** A hang és vibráció lejátszására ad egy keretet, így az az adott eszköztől független hívható. Ezt az interfészt eszközspecifikusan implementáljuk.   

##### SoundAndVibrationWindows
**Sztereotípia:** egyed  
**Példány:** soundAndVibrationWindows  
**Feladat:** Windows specifikus megvalósítása a SoundAndVibration interfésznek  

#### 2.2.1 Menü  

![Menü osztálymodell](/readme_resources/menuobjectmodell2.jpg?raw=true "Menü osztálymodell")

#### 2.2.2 Szerver

![Szerver osztálymodell](/readme_resources/server_class_model.png?raw=true "Szerver osztálymodell")

### 2.3 Adatbázis terv

![Adatbázis terv](/readme_resources/db_model_relational.png?raw=true "Adatbázis terv")

### 2.4 RESTful web-szolgáltatások

<table>
<tr><th>Szolgáltatás</th><th>Játék indítása - egy játékos</th></tr>
<tr><td>URI</td><td><tt>/new-board/{difficulty}/{number-of-fields}</tt></td></tr>
<tr><td>HTTP verb</td><td>GET</td></tr>
<tr><td>response</td><td>
<pre>{
"board" : [ {
  "word"  : &lt;string> ,
  "image" : &lt;string> } ]
}</pre></td></tr>
<tr><td>Megjegyzések</td><td>
<tt>difficulty</tt> eleme <tt>{"easy", "normal", "hard"}</tt>;<br/>
<tt>number-of-fields</tt> darab szó-kép (transzformált) párt generál;<br/>
<tt>image</tt>: base64 kódolt JPEG
</td></tr>
</table>

<table>
<tr><th>Szolgáltatás</th><th>Scoreboard lekérdezése</th></tr>
<tr><td>URI</td><td><tt>/scoreboard/{mode}/{difficulty}</tt></td></tr>
<tr><td>HTTP verb</td><td>GET</td></tr>
<tr><td>response</td><td>
<pre>{
"scoreboard" : [ {
  "player"    : &lt;string> ,
  "score"     : &lt;number> ,
  "timestamp" : &lt;number> } ]
}</pre></td></tr>
<tr><td>Megjegyzések</td><td>
<tt>mode</tt> eleme <tt>{"normal", "time"}</tt>;<br/>
<tt>difficulty</tt> eleme <tt>{"easy", "normal", "hard"}</tt>;<br/>
<tt>timestamp</tt>: másodpercek 2015. jan. 1. 00:00:00 óta
</td></tr>
</table>

<table>
<tr><th>Szolgáltatás</th><th>Játékos eredményének regisztrálása</th></tr>
<tr><td>URI</td><td><tt>/register-score/{mode}/{difficulty}</tt></td></tr>
<tr><td>HTTP verb</td><td>PUT</td></tr>
<tr><td>request body</td><td><pre>{
"player" : &lt;string> ,
"score"  : &lt;number>
}</pre></td></tr>
<tr><td>response</td><td>egyszerű HTML response code</td></tr>
<tr><td>Megjegyzések</td><td>
<tt>mode</tt> eleme <tt>{"normal", "time"}</tt>;<br/>
<tt>difficulty</tt> eleme <tt>{"easy", "normal", "hard"}</tt>
</td></tr>
</table>

<table>
<tr><th>Szolgáltatás</th><th>Új pályaelem generálása (endless módhoz)</th></tr>
<tr><td>URI</td><td><tt>/new-tile/{difficulty}</tt></td></tr>
<tr><td>HTTP verb</td><td>GET</td></tr>
<tr><td>response</td><td><pre>{
"word"  : &lt;string> ,
"image" : &lt;string>
}</pre></td></tr>
<tr><td>Megjegyzések</td><td>
<tt>difficulty</tt> eleme <tt>{"easy", "normal", "hard"}</tt>;<br/>
<tt>image</tt>: base64 kódolt JPEG
</td></tr>
</table>

### 2.5 Multi-player protokoll
A multi-player funkcionalitás modellje egy véges állapot-automata, aminek
* állapotai a játékállapotok funkcionálisan egybe tartozó részhalmazai,
* az állapot átmenetek a felhasználó tevékenysége vagy eltelt idő-intervallum által kiváltott TCP/IP üzenetváltások.

#### 2.5.1 Állapotok
<table>
<tr><th>Állapot</th><th>Belépési pont</th></tr>
<tr><td>Kliens jellemzők</td><td>A kliens a multi-player játékhoz való csatlakozás előtt áll.</td></tr>
<tr><td>Szerver jellemzők</td><td>A szerver várja a csatlakozni kívánó klienseket.</td></tr>
<tr><td>Állapot átmenetek</td><td><ul>
<li>Regisztráció</li></ul></td></tr>
</table>

<table>
<tr><th>Állapot</th><th>Várakozás párosításra</th></tr>
<tr><td>Kliens jellemzők</td><td>A kliens várakozik a szerver válaszára.</td></tr>
<tr><td>Szerver jellemzők</td><td>A szerver fogadja a további csatlakozási kérelmeket. A megküldött játék attribútumok szerint különböző sorokban várakoztatja a klienseket, amíg nem sikerül párosítani őket.</td></tr>
<tr><td>Állapot átmenetek</td><td><ul>
<li>Új "Harc a pontokért" játék</li>
<li>Új "Időzítő" játék - a kliens kezd</li>
<li>Új "Időzítő" játék - az ellenfél kezd</li></ul></td></tr>
</table>

<table>
<tr><th>Állapot</th><th>Harc a pontokért</th></tr>
<tr><td>Kliens jellemzők</td><td>A játékos sorban megoldja a feladatokat.</td></tr>
<tr><td>Szerver jellemzők</td><td>A szerver minden megoldott feladatnál értesíti az ellenfelet a játékos pontállásáról.</td></tr>
<tr><td>Állapot átmenetek</td><td><ul>
<li>"Harc a pontokért" értesítő küldése</li>
<li>"Harc a pontokért" értesítő fogadása</li>
<li>Minden feladat megoldásra került küldése</li></ul></td></tr>
</table>

<table>
<tr><th>Állapot</th><th>Időzítő - a játékos lép</th></tr>
<tr><td>Kliens jellemzők</td><td>A játékoson van a sor, hogy megoldjon egy feladatot. Közben a rendelkezésére álló idő folyamatosan csökken.</td></tr>
<tr><td>Szerver jellemzők</td><td>A szerver várakozik egy feladat megoldására vagy az idő letelésére.</td></tr>
<tr><td>Állapot átmenetek</td><td><ul>
<li>"Időzítő" értesítő küldése és új elem fogadása</li>
<li>A rendelkezésre álló idő elfogyott</li></ul></td></tr>
</table>

<table>
<tr><th>Állapot</th><th>Várakozás "Harc a pontokért" módban</th></tr>
<tr><td>Kliens jellemzők</td><td>A kliens várakozik arra, hogy ellenfele is végezzen.</td></tr>
<tr><td>Szerver jellemzők</td><td>A szerver várakozik a kliens ellenfelére.</td></tr>
<tr><td>Állapot átmenetek</td><td><ul>
<li>"Harc a pontokért" értesítő fogadása</li>
<li>Minden feladat megoldásra került fogadása</li></ul></td></tr>
</table>

<table>
<tr><th>Állapot</th><th>Várakozás "Időzítő" módban</th></tr>
<tr><td>Kliens jellemzők</td><td>A kliens várakozik arra, hogy ellenfele megoldjon egy feladatot, vagy lejárjon az ideje.</td></tr>
<tr><td>Szerver jellemzők</td><td>A szerver várakozik a kliens ellenfelére.</td></tr>
<tr><td>Állapot átmenetek</td><td><ul>
<li>"Időzítő" módban új elem fogadása</li>
<li>Nyertél értesítő fogadása</li></ul></td></tr>
</table>

<table>
<tr><th>Állapot</th><th>Kilépési pont</th></tr>
<tr><td>Kliens jellemzők</td><td>A kliens megjeleníti a multi-player játék eredményét.</td></tr>
<tr><td>Szerver jellemzők</td><td>A szerver bontja a kapcsolatot a kliensekkel.</td></tr>
<tr><td>Állapot átmenetek</td><td>-</td></tr>
</table>

#### 2.5.2 Állapot átmenetek
<table>
<tr><th>Állapot átmenet</th><th>Regisztráció</th></tr>
<tr><td>Kiinduló állapot</td><td>Belépési pont</td></tr>
<tr><td>Következő állapot</td><td>Várakozás párosításra</td></tr>
<tr><td>Kiváltó esemény</td><td>A felhasználó új multi-player játékhoz való csatlakozást választotta a menüben.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A kliens csatlakozik a <tt>http://nipglab09.inf.elte.hu:8001</tt> porthoz TCP/IPv4 módban, majd üzenetet küld a következő formátumban:<br/><pre>{
"name"       : &lt;string>,
"mode"       : &lt;"timer"|"fight">,
"difficulty" : &lt;"easy"|"normal"|"hard">,
"layout"     : &lt;"rectangle"|"diamond"|"fish">
}</pre></td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>Új "Harc a pontokért" játék</th></tr>
<tr><td>Kiinduló állapot</td><td>Várakozás párosításra</td></tr>
<tr><td>Következő állapot</td><td>Harc a pontokért</td></tr>
<tr><td>Kiváltó esemény</td><td>A szerver sikeresen párosít két "Harc a pontokért" módban regisztráló játékost, azonos nehézségi szinten és layouttal.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A szerver legenerál egy új játékot, amit a következő formátumban küld a klienseknek:<br/><pre>{
"opponent" : &lt;string>,
"layout"   : {
  "height" : &lt;number>,
  "width"  : &lt;number>,
  "active_fields" : [ &lt;true|false> ] },
"board"    : [ {
  "row_index" : &lt;number>,
  "col_index" : &lt;number>,
  "image"     : &lt;string>,
  "word"      : &lt;string> } ],
"to_find"  : [ &lt;string> ]
}</pre><br/>ahol <tt>image</tt> base64 kódolású JPEG kép.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>Új "Időzítő" játék - a kliens kezd</th></tr>
<tr><td>Kiinduló állapot</td><td>Várakozás párosításra</td></tr>
<tr><td>Következő állapot</td><td>Időzítő - a játékos lép</td></tr>
<tr><td>Kiváltó esemény</td><td>A szerver sikeresen párosít két "Időzítő" módban regisztráló játékost, azonos nehézségi szinten és layouttal. Kisorsolta, hogy ez a kliens kezd.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A szerver legenerál egy új játékot, amit a következő formátumban küld a kliensnek:<br/><pre>{
"opponent"  : &lt;string>,
"layout"    : {
  "height" : &lt;number>,
  "width"  : &lt;number>,
  "active_fields" : [ &lt;true|false> ] },
"board"     : [ {
  "row_index" : &lt;number>,
  "col_index" : &lt;number>,
  "image"     : &lt;string>,
  "word"      : &lt;string> } ],
"to_find"   : [ &lt;string> ],
"your_turn" : true
}</pre><br/>ahol <tt>image</tt> base64 kódolású JPEG kép.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>Új "Időzítő" játék - az ellenfél kezd</th></tr>
<tr><td>Kiinduló állapot</td><td>Várakozás párosításra</td></tr>
<tr><td>Következő állapot</td><td>Várakozás "Időzítő" módban</td></tr>
<tr><td>Kiváltó esemény</td><td>A szerver sikeresen párosít két "Időzítő" módban regisztráló játékost, azonos nehézségi szinten és layouttal. Kisorsolta, hogy ennek a kliensnek az ellenfele kezd.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A szerver legenerál egy új játékot, amit a következő formátumban küld a kliensnek:<br/><pre>{
"opponent"  : &lt;string>,
"layout"    : {
  "height" : &lt;number>,
  "width"  : &lt;number>,
  "active_fields" : [ &lt;true|false> ] },
"board"     : [ {
  "row_index" : &lt;number>,
  "col_index" : &lt;number>,
  "image"     : &lt;string>,
  "word"      : &lt;string> } ],
"to_find"   : [ &lt;string> ],
"your_turn" : false
}</pre><br/>ahol <tt>image</tt> base64 kódolású JPEG kép.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>"Harc a pontokért" értesítő küldése</th></tr>
<tr><td>Kiinduló állapot</td><td>Harc a pontokért</td></tr>
<tr><td>Következő állapot</td><td>Harc a pontokért</td></tr>
<tr><td>Kiváltó esemény</td><td>A játékos megoldott egy feladatot, de nem az utolsót.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A kliens a feladat megoldásának megfelelően frissíti saját játékállapotát és a GUI-t, majd a következő üzenetet küldi a szervernek:<br/><pre>{
"row_index" : &lt;number>,
"col_index" : &lt;number>,
"score"     : &lt;number>
}</pre><br/>ahol <tt>*_index</tt> a megoldott feladat koordinátái, <tt>score</tt> az eddig megszerzett pontok.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>"Harc a pontokért" értesítő fogadása</th></tr>
<tr><td>Kiinduló állapot 1</td><td>Harc a pontokért</td></tr>
<tr><td>Következő állapot 1</td><td>Harc a pontokért</td></tr>
<tr><td>Kiinduló állapot 2</td><td>Várakozás "Harc a pontokért" módban</td></tr>
<tr><td>Következő állapot 2</td><td>Várakozás "Harc a pontokért" módban</td></tr>
<tr><td>Kiváltó esemény</td><td>A játékos ellenfele megoldott egy feladatot, de nem az utolsót.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A szerver a következő üzenettel értesíti a klienst a játékállásról:<br/><pre>{
"purpose"        : "update",
"opponent_score" : &lt;number>
}</pre><br/>A kliens frissíti a GUI-t.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>Minden feladat megoldásra került küldése</th></tr>
<tr><td>Kiinduló állapot</td><td>Harc a pontokért</td></tr>
<tr><td>Következő állapot</td><td>Várakozás "Harc a pontokért" módban</td></tr>
<tr><td>Kiváltó esemény</td><td>A játékos megoldotta az utolsó feladatot.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A kliens megjeleníti az elért eredményt, és a következő üzenetet küldi a szervernek:<br/><pre>{
"row_index" : &lt;number>,
"col_index" : &lt;number>,
"score"     : &lt;number>
}</pre><br/>ahol <tt>*_index</tt> a megoldott feladat koordinátái, <tt>score</tt> az eddig megszerzett pontok.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>Minden feladat megoldásra került fogadása</th></tr>
<tr><td>Kiinduló állapot</td><td>Várakozás "Harc a pontokért" módban</td></tr>
<tr><td>Következő állapot</td><td>Kilépési pont</td></tr>
<tr><td>Kiváltó esemény</td><td>A játékos ellenfele megoldotta az utolsó feladatot.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A szerver a következő üzenettel értesíti a klienst a játékállásról:<br/><pre>{
"purpose"        : "opponent_finished",
"opponent_score" : &lt;number>
}</pre><br/>A kliens frissíti a GUI-t.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>"Időzítő" értesítő küldése és új elem fogadása</th></tr>
<tr><td>Kiinduló állapot</td><td>Időzítő - a játékos lép</td></tr>
<tr><td>Következő állapot</td><td>Várakozás "Időzítő" módban</td></tr>
<tr><td>Kiváltó esemény</td><td>A játékos megoldott egy feladatot.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A kliens kiveszi a megoldott feladatot a feladatlistából, leállítja saját óráját, elindítja az ellenfélét, és értesíti a szervert a megoldott feladatról:<br/><pre>{
"purpose" : "solved_task",
"row_index" : &lt;number>,
"col_index" : &lt;number>
}</pre><br/>A szerver új képet generál a megoldott helyére, valamint küld egy új megoldandó feladatot:<br/><pre>{
"purpose"  : "update",
"new_tile" : {
  "row_index" : &lt;number>,
  "col_index" : &lt;number>,
  "image"     : &lt;string>,
  "word"      : &lt;string> },
"new_task" : &lt;string>
}</pre><br/>ahol <tt>image</tt> base64 kódolású JPEG kép.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>"Időzítő" módban új elem fogadása</th></tr>
<tr><td>Kiinduló állapot</td><td>Várakozás "Időzítő" módban</td></tr>
<tr><td>Következő állapot</td><td>Időzítő - a játékos lép</td></tr>
<tr><td>Kiváltó esemény</td><td>A játékos ellenfele megoldott egy feladatot.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A szerver új képet generál az ellenfél által megoldott helyére, valamint küld egy új megoldandó feladatot:<br/><pre>{
"purpose"  : "update",
"new_tile" : {
  "row_index" : &lt;number>,
  "col_index" : &lt;number>,
  "image"     : &lt;string>,
  "word"      : &lt;string> },
"new_task" : &lt;string>
}</pre><br/>ahol <tt>image</tt> base64 kódolású JPEG kép. A kliens kiveszi a megoldott feladatot a feladatlistából, és beteszi az új feladatot. A megoldott képet helyettesíti az újjal. Leállítja az ellenfél óráját, és elindítja a sajátját.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>A rendelkezésre álló idő elfogyott</th></tr>
<tr><td>Kiinduló állapot</td><td>Időzítő - a játékos lép</td></tr>
<tr><td>Következő állapot</td><td>Kilépési pont</td></tr>
<tr><td>Kiváltó esemény</td><td>A játékos rendelkezésére álló idő elfogyott, ezzel elveszítette a játékot.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A kliens értesíti a szervert az idő lejártáról:<br/><pre>{
"purpose"  : "time_out"
}</pre>A kliens megjeleníti az elért eredményt.</td></tr>
</table>

<table>
<tr><th>Állapot átmenet</th><th>Nyertél értesítő fogadása</th></tr>
<tr><td>Kiinduló állapot</td><td>Várakozás "Időzítő" módban</td></tr>
<tr><td>Következő állapot</td><td>Kilépési pont</td></tr>
<tr><td>Kiváltó esemény</td><td>A játékos ellenfelének rendelkezésére álló idő elfogyott, ezzel a játékos nyert.</td></tr>
<tr><td>Végrehajtandó lépések</td><td>
A szerver értesíti a klienst az idő lejártáról:<br/><pre>{
"purpose"  : "you_won"
}</pre>A kliens megjeleníti az elért eredményt.</td></tr>
</table>

### 2.6 Dinamikus működés  
#### 2.6.1 Menü  
![Menü szekvenciadiagram](/readme_resources/menusequencemodell1.jpg?raw=true "Menü szekvenciadiagram")

#### 2.6.2 Scoreboard
![Scoreboard szekvenciadiagram](/readme_resources/sequence.jpg?raw=true "Scoreboard szekvenciadiagram")

#### 2.6.3 Multiplayer játék indítása
![Multiplayer szekvenciadiagram](/readme_resources/multisequence.jpg?raw=true "Multiplayer szekvenciadiagram")

#### 2.6.4 Sakk alapú időkorlátos játékmód
![Időkorlátos játékmód szekvenciadiagram](/readme_resources/chessMethodSequence.jpg?raw=true "Időkorlátos játékmód szekvenciadiagram")

### 2.7 Felhasználói felület modell  
A játék alapvetően mobil platformokra van tervezve, érintéssel működik. Minden ablak teljes képernyős, mindig a legutoljára felnyitott ablak aktív. A menükben vissza lehet menni bármelyik előző ablakra. Játék közben a pause menüt lehet megnyitni, a főmenübe visszajutni csak az aktuális játék megszakításával lehet.

![Főmenü](/readme_resources/mainmenu.jpg?raw=true "Főmenü")  
![Játékmód választás](/readme_resources/gamemenu.jpg?raw=true "Játékmód választás")  
![Játékmenet](/readme_resources/gamemode.jpg?raw=true "Játékmenet")  
![Játékmenet, pálya template](/readme_resources/gamemode2.jpg?raw=true "Játékmenet, pálya template")  

Kész programról képernyőképek

![Főmenü](/screens/mainmenu.png?raw=true "Főmenü")  
![Egyjátékos mód](/screens/singleplayer.png?raw=true "Egyjátékos mód")  
![Többjátékos mód](/screens/multiplayer.png?raw=true "Többjátékos mód")
![Scoreboard](/screens/scoreboard.png?raw=true "Scoreboard")  
![Tutorial](/screens/tutorial.png?raw=true "Tutorial")  
![Játék](/screens/singleplay.png?raw=true "Játék")  
![Játék vége](/screens/gameover.png?raw=true "Játék vége")  
### 2.8 Részletes programterv

**Tábla**  
*mezőHozzáadás* – hozzáad egy mezőt a Tábla osztály összesMező nevű listájához.  
*Tábla* – egyéni konstruktor, mely bemeneti paraméterként képek egy listáját várja.  
*mezőLekérés* – sor és oszlopindex megadása után az összesMező egy megfelelő elemét adja vissza.  
*táblaKészítés* – egy nehézségi szint megadása után beállítja a sor- és oszlopszámot, majd feltölti az összesMező listát véletlenszerű képekkel.  
*mezőCsere* – kicseréli a paraméterben megadott mező képét egy random másikra  
**Mező**  
*Mező* – egyéni konstruktor, ami feltölti a mezőt a paraméterekben megadott adatokkal  
**JátékModell**  
*játékModell* – egyéni konstruktor, amely bemeneti paraméternek egy KépAdatbázis objektumot vár.  
*inizializálás* – létrehozza a felső menüt, beállítja az órát, létrehozza a főmenüt, illetve egy táblát.  
*tartalomBetöltés* – MonoGame függvény, ami az aktuális tartalmat tölti be.  
*frissítés* – MonoGame függvény, jelen esetben a játék közbeni egér kattintására figyel.  
*kirajzolás* – kirajzolja az ablakon megjelenő összes elemet  
*felsőMenükészítés* – meghívja a felső menü kirajzolásáért felelős osztály rajzolás metódusát.  
**FelsőMenü**  
*komponensLekérés* – visszaadja egy listában a középső hátteret valamint az időzítő hátterét egy RajzolhatóJátékKomponens típusú objektumként. Ez a típus az XNA sajátja.  
*kirajzolás* – hozzáadja a JátékMenedzser típusú objektumhoz a fenti két komponenst, valamint egy Menü gombot.  
*időzítőSzövegBeállítás* – beállítja a kiírandó időt.  
**TáblaRajzolás**  
*TáblaRajzolás* – beállítja a szükséges változókat a kirajzoláshoz. Ezek az egyes elemek 	szélessége és magassága, illetve a kirajzolás kezdőpontja.  
*frissítés* – sor- és oszlopindex kiszámítása az egérkattintásból, és a kattintás esemény meghívása  
*kirajzolás* – a mezőket rajzolja ki sor és oszlopindexek alapján. A sorok száma a tábla X-, az oszlopok száma a tábla Y adattagjaitól függ. A mezők négyzetekbe kerülnek.  
**PontRajzoló**  
*TartalomBetöltés* – beálíttja a szükséges kirajzolandó elemet, ez a háttér.  
*Rajzolás* – kirajzolja a pontokat és a kombót  
**IdőRajzoló**  
*TartalomBetöltés* – beállítja a szükséges kirajzolandó elemet, ez a háttér.  
*Rajzolás* – kirajzolja a hátteret és rá az időt  
*IdőSzöveg* – a kirajzolandó idő szövegesen.  
**MegtalálandóRajzoló**  
*MegtalálandóLista* – beállítja/visszaadja a megtalálandó képek listáját.  
*TartalomBetöltés* – beállítja a hátteret.  
*Rajzolás* – kirajzolja a megtalálandó listát.  
**Kép**  
*képTextúra*  
*név*  
**SzintMenedzser**  
*elteltIdőKezelő* – akkor hívódik meg, mikor a játékban eltelik egy másodperc. Paraméterben átadja az eltelt / maradék időt.  
*megálltIdőKezelő* – akkor hívódik meg, mikor az idő letelik.  
*SzintMenedzser* – egyéni konstruktor, amely beállítja az időzítőt.  
*újJáték* – egy új játék megadott paraméterek alapján történő indításáért felel. A paraméterek a következők: játékmód, nehézségi szint és az összes kép listája. A függvény egy táblával tér vissza.  
*próbaMezőKattintás* – a paraméterben megadott mezőre rákattint, de csak akkor, ha az elérhető. Illetve átállítja a mezőt elérhetetlenné, ha olyan játékmódot játszik a játékos.  
*játékVége* – befejezi a játékot, azaz lenullázza a számlálót.  
*eltelt_Idő* – ez végzi a konkrét számlálást minden egyes másodpercben.  
**Pontozó**  
*Pontozó* – lenullázza a pontokat és a kombót és beállítja a kombó felezési idejét.  
*hozzáad* – Hozzáad megadott pontot az osztályhoz kombó szorzójával együtt.  
*kombóBeállítás* - Hozzáad a kombóhoz egyet, ha még nem telt le az előző kombó ideje. Szorozza a kombót 2-vel, ha még nem több, mint 16.  
*időzítőEltelt* – Az aktuálisan futó időzítő letelt. Ilyenkor felezi a kombót, ha még nem 1. Újraindítja az időzítőt.  
*időzítőMegállítás* – Megállítja az aktuálisan futó időzítőt.  
**KépAdatbázis**  
*összesKép* – visszaadja egy listában az összes képet.  
*tartalomBetöltés* – betölti egy fájlból az összes kép nevét.  
*képBetöltés* – betölt egy képet Texture2D típusú objektumként.  
**Mentés**  
*mentés* – kiírja egy fájlba az adott tábla adatait: sorok és oszlopok számát, valamint az összes mezőt.  
*betöltés* – beolvassa fájlból az adott tábla adatait és felépít belőle egy új táblát az összes kép listájának segítségével.  
**KépekMegtalálásra**  
*Megtalálásra* – a megtalálandó képek listája.  
*próbaMezőKeresés* – a paraméterben megadott mezőn lévő képet megkeresi a 	megtalálandó képek listájában. Ha megtalálja, akkor törli a képet a megtalálandók 	listájából. Végtelen mód esetében rak be egy új megtalálandó képet, és ha elfogytak a 	megtalálandó képek, akkor meghív egy eseményt.  
*újKépHozzáadása* – hozzáad a megtalálandók listájához egy új képet  
*szöveggéAlakít* – Átalakítja a megtalálandó képek listáját szövegre úgy, hogy a 	többször szereplőket x2, x3 szorzóval látja el.  
*újKépekListájaKészítés* – elkészíti a megtalálandó képek osztályát az elkészített 	táblából nehézség szerint.  
**Transzformátor**  
*Transzformátor* – egyéni konstruktor, a paraméterekkel beállítja a példányt.  
*transzformálás* – absztrakt függvény, egy textúrán végez el egy később definiált transzformálást  
**Homályosít**  
*transzformálás* – elvégzi a textúrán a homályosítást  
**SzínTranszformáció**  
*transzformálás* – beszínezi a megadott textúrát  
**TükrözőTranszformáció**  
*transzformálás* – tükrözi véletlenszerű irányba a textúrát  
**ForgatóTranszformáció**  
*transzformálás* – elforgatja a textúrát véletlenszerű irányba  

A részletes programterv kiegészítése a következő követelményekkel:
* Tutorial, Scoreboard
* Kommunikáció a szerverrel  

**SzerverKommunikator**  
*utasitasKuldes* - egy általános függvény, ami a paraméterét JSON formátumra alakítja át, majd elküldi a szervernek. Megvárja a választ és visszatér a válasszal szinkron módon (Aszinkron kell a függvényt meghívni)  
**ToplistaKommunikator**  
*getTopTenScores* - lekérdezi a szervertől a megadott játékmódhoz tartozó 10 legtöbb pontot elért játékos nevét pontszámmal együtt  
*setGamemode* - beállítja a játékmódot, amihez lekérjük a toplistát  
**ToplistaRajzolo**  
*Draw* - kirajzolja a toplistát a már meglévő adatokból  
**Tutorial**  
*isFirstGame* - visszaadja, hogy ez az első indítása-e a játéknak  
*startTutorial* - elindítja a tutorialt egy új játékmód formájában  
**Menu**  
*Draw* - kirajzolja a menüt  

## 3.	Implementáció  
### 3.1 Fejlesztőeszközök  
A fejlesztéshez Visual Studio szerkesztőt használtunk, Monogame keretrendszerrel. Kódmegosztáshoz a github.com oldalt használtuk, ami Git verziókezelést alkalmaz.
Minden platformhoz külön projektet hoztunk létre, és van egy közös projekt a Common. Ebben a Commonban találhatóak a közös kódot tartalmazó fizikai fájlok, a többi projektben ha ezekre szükség van, linkelt fájlokként adjuk hozzá az adott projekthez.
Kalmár Dániel feladata a menü rendszere, illetve a MonoGame alapjainak megírása, Birkás Gábor feladata a mentés és betöltés, a beállítások és a képletöltő absztrakciója. Rabi Péter feladata a különböző transzformációk megvalósítása, Csatári Albert feladata a játékmodell, tábla, mezők, pontozás és a játékmódok implementálása.
### 3.2 Forráskód, futtatható kód  
Egy publikus [github](http://github.com/csatari/pixeek) fiókon van kezelve. 
### 3.3 Alkalmazott kódolási szabványok  
A forráskódban az osztálynevek egybeírva, minden szó nagy betűvel kezdődik. A tulajdonságokra (C# property) szintén ezt az elvet alkalmaztuk, egybeírva a nevek, mindegyik nagy betűvel kezdődik. A függvénynevek, illetve a változókra pedig azt az elvet alkalmaztuk, hogy az összes név egybe van írva, mindegyik szó nagy betűvel kezdődik, de az első kicsivel.
A forráskódban mindig angol, beszédes neveket használtunk.

## 4.	Tesztelés
### 4.1 Black box teszt

* A programot elindítva kattinthatunk a Single Player, Multi player, Scoreboard, Tutorial, vagy az Exit gombra. Exit esetén a program sikeresen terminál.

Scoreboard:

* A program sikeresen megjeleníti az elért eredményeket
* Bármilyen játékmód, vagy nehézség beállításához megmutatja az eredményeket
* Ha nincs eredmény nem mutat semmit
* Betöltés alatt kiírja, hogy Loading...

Tutorial:

* Átlép sikeresen az egyjátékos mód képernyőjére
* Megjelenik a tutorial leírása
* Látszódik a menü összes eleme
* Kattinható a menü összes eleme
* Ha máshova kattintunk, jön a következő tutorial
* Elindítható a Tutorial végigjátszása nélkül is a játék
* A tutorial elemek helyesen mutatják be az adott elemeket
* A tutorial elemek rendezett tematika szerint mennek végig az elemeken
* Elindul a játék és a Tutorial továbbvisz
* A játék nem áll, amíg a tutorial megy
* A tutorial kéri, hogy találjunk meg egy az alsó részből található elemet
* Elindul az óra
* Ha mást találunk meg, nem megy tovább a tutorial
* Ha eltaláljuk, amit kér, kiírja, hogy have fun
* Véget ér a tutorial

Multiplayer:

* Megjelenik a két játékmód
* Play gombra nem történik semmi //TODO legyen multiplayer

Single player:

* A Single player gombra kattintva megjelenik egy játékmód- illetve egy nehézségi szint választó képernyő. Itt állíthatjuk be a hangot és a vibrációt is, valamint indíthatjuk el a tényleges játékot.
* A játékmódokat a Normal, Endless és Time gombokkal választhatjuk meg, amelyekhez társítunk egy nehézségi szintet is. A Play gombra kattintva elindul a játék.  

Normal – Easy – Music: ON – Vibration: OFF  
* A játékot elindítva az időzítő megfelelően működik – növeli az eltelt időt. 
* Kezdetben a score 0. 
* Jelenlegi esetben négy különböző képet kell megtalálni. 
* Végignyomva minden nem megfelelő képet, nem kapunk pontokat, és ezt megfelelő hangjelzés is kíséri minden esetben. 
* Jó mezőt kiválasztva pontokat kapunk. 
* Ha megfelelően gyorsan találunk meg egy adott képet, akkor a combo érték megfelelő szorzóra emelkedik. 
* Minden felsorolt képet megtalálva rendeltetésszerűen előugrik a Game Over képernyő, ahol megjelenik a megfelelő pontszám, illetve időtartam. 
* Ha játék közben a Menu gombra kattintunk, akkor elkészül egy mentés az aktuális állásról, melyet később folytatni tudunk.   

Endless – Hard – Music: OFF – Vibration: OFF  
* A pontozás és az időszámítás ebben az esetben is megfelelően működik. 
* A Hard nehézségi szintnek megfelelően sokkal több képet kell megtalálnunk, egyes elemekből többet is. 
* Rossz mezőre kattintva ebben az esetben sem történik semmi, jó kép megtalálása esetén azonban egy új, véletlenszerűen kiválasztott kép kerül a helyére. Ez az esemény új feladatokkal is párosul. 
* Hangeffektek nincsenek.  

Time – Normal – Music: OFF – Vibration: OFF  
* Ebben az esetben 30 másodpercünk van megtalálni a kívánt elemeket, így az óra megfelelően 5-től számol vissza. 
* Elérve a 0-t, amennyiben nem találtuk meg az összes képet, egy Game Over képernyő jelenik meg, megfelelő feliratokkal. 
* Hangeffektek a beállítás miatt itt sincsenek.
* A játéktábla alakjai megfelelően működtetik a mezőket – nem lehet üres területre kattintani, és kattintásra minden mező helyesen cselekszik.  
* A képeken véletlenszerűen működnek a különböző transzformációk, melyek kombinációi - a követelmény leíráshoz igazodva – alkalmazkodnak a nehézségi szintekhez.

### 4.2 További tesztelések
A további tesztelésekhez unit teszteseteket alkamaztunk, amik a program indításakor lefutnak. Ezek a tesztesetek megpróbálják lefedni az összes lehetőséget, amit egy adott fügvénnyel végezni lehet, és azt vizsgálják, hogy a függvény elvégzése után a megfelelő állapotot kapjuk-e vissza.
Ezek a tesztesetek a következő útvonalon találhatóak: A főmappa, azaz az src mappán belül a Testing mappában a Testing a menü vizsgálatáért, a GameTesting a pályák létrehozásáért, kattintásáért, a keresendő képekért és a pontozási rendszerért felelős.

