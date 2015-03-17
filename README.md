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
*	új témák (játékon belül letölthető a szerverről)
*	online ranglisták
*	tutorial

### 1.2 Szakterületi fogalomjegyzék
- hidden object játék - olyan játék, amiben egy statikus háttér fölött, abba beleolvadó objektumokat kell megkeresni
- casual gamer -  az az átlagos mobiltelefon-használó, akit az egyszerű, rövid játékmenetű, figyelemelvonó játékok érdekelnek
- gameplay prototype - alap funkcionalitású játék, amin látható és tesztelhető a játékmenet maga, minden egyéb kiegészítés nélkül

### 1.3 Használatieset-modell, funkcionális követelmények

![Használati eset modell](/readme_resources/usecase1.jpg?raw=true "Használati eset modell")

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
* A téma kiválasztható az új játék kezdésekor, a témához szükséges erőforrások a szerverről töltődnek le
* Ranglista
* Tutorial – első játékkor ajánlja fel, illetve a menüből is elérhető később kérésre. Egy példajátékon vezet végig.


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
A megosztott projektnek nincsen output assemblyje, tehát ha a fordított kódot DLL-
ként szeretnénk megosztani, akkor PCL-t kell használni.

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

## 2.	Tervezés
### 2.1 A program architektúrája
A program legfontosabb elemei:
Modell – PCL-ben
2 fő osztály, ami a két külön nézet tulajdonságait tartalmazza:
* Jatek osztály
* Menu osztály
Nézet:
* JatekAblak
* BeállításokAblak
A program architektúrája úgy lesz kialakítva, hogy a Xamarin keretrendszer által támogatott legyen, azaz minden platform külön projekt, illetve van egy közös kódot tartalmazó PCL.

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

#### 2.2.1 Menü
![Menü osztálymodell](/readme_resources/menuobjectmodell2.jpg?raw=true "Menü osztálymodell")

### 2.3 Adatbázis terv
A program nem tartalmaz komplex adatbázist. A pontszámokat és képeket tároló adatstruktúrák a többi modellen vannak megjelenítve.

### 2.4 Dinamikus működés  
#### 2.4.1 Menü  
![Menü szekvenciadiagram](/readme_resources/menusequencemodell1.jpg?raw=true "Menü szekvenciadiagram")

### 2.5 Felhasználói felület modell  
A játék alapvetően mobil platformokra van tervezve, érintéssel működik. Minden ablak teljes képernyős, mindig a legutoljára felnyitott ablak aktív. A menükben vissza lehet menni bármelyik előző ablakra. Játék közben a pause menüt lehet megnyitni, a főmenübe visszajutni csak az aktuális játék megszakításával lehet.

![Főmenü](/readme_resources/mainmenu.jpg?raw=true "Főmenü")  
![Játékmód választás](/readme_resources/gamemenu.jpg?raw=true "Játékmód választás")  
![Játékmenet](/readme_resources/gamemode.jpg?raw=true "Játékmenet")  
![Játékmenet, pálya template](/readme_resources/gamemode2.jpg?raw=true "Játékmenet, pálya template")  

### 2.6 Részletes programterv

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

* A programot elindítva kattinthatunk a New Game, vagy az Exit gombra. Exit esetén a program sikeresen terminál.
* A New Game gombra kattintva megjelenik egy játékmód- illetve egy nehézségi szint választó képernyő. Itt állíthatjuk be a hangot és a vibrációt is, valamint indíthatjuk el a tényleges játékot.
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
Ezek a tesztesetek a következő útvonalon találhatóak: A főmappa, azaz az src mappán belül a Testing mappában a Testing a menü vizsgálatáért, a GameTesting a pályák létrehozásáért, kattintásáért, a keresendő képekért és a pontozási rendszerért, a TransformationTest pedig a 4 előforduló transzformáció (forgatás, tükrözés, színezés, homályosítás) helyes lefutásának ellenőrzéséért felelős.

