# Звіт з лабораторної роботи №14

- **Тема:** Аналіз SRP/OCP у відкритих репозиторіях.
- **Мета:** Дослідити, як принципи SRP та OCP застосовуються в реальних проєктах, і підготувати короткий практичний аналіз прикладів рефакторингу.

---

## Репозиторії

1. [dotnet/eShop](https://github.com/dotnet/eShop/tree/913acea66708a59595351174cee9b29f37179d29) — інтернет-магазин (вебзастосунок).
2. [JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json/tree/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa) — бібліотека серіалізації JSON.

Посилання ведуть на конкретні коміти, щоб номери рядків не зсувалися.

## Вдале застосування SRP

**1. eShop: [CancelOrderCommandHandler](https://github.com/dotnet/eShop/blob/913acea66708a59595351174cee9b29f37179d29/src/Ordering.API/Application/Commands/CancelOrderCommandHandler.cs#L4-L30)**
- Де видно: клас лише скасовує замовлення (знайти, змінити статус, зберегти), кожна інша команда має свій обробник.
- Підтримка: клас маленький, його легко читати й тестувати, зміна скасування не зачіпає інші команди.
- Нова функція: нова команда отримує новий обробник, цей клас не змінюється.

**2. Newtonsoft.Json: [VersionConverter](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Converters/VersionConverter.cs#L35-L105)**
- Де видно: клас лише перетворює `System.Version` у рядок JSON і назад.
- Підтримка: помилку з версіями шукають в одному файлі на 100 рядків.
- Нова функція: підтримка іншого типу робиться окремим конвертером, цей клас не змінюється.

## Порушення SRP

**3. eShop: [CatalogApi](https://github.com/dotnet/eShop/blob/913acea66708a59595351174cee9b29f37179d29/src/Catalog.API/Apis/CatalogApi.cs)**
- Де видно: один статичний клас на 468 рядків поєднує [маршрути](https://github.com/dotnet/eShop/blob/913acea66708a59595351174cee9b29f37179d29/src/Catalog.API/Apis/CatalogApi.cs#L13-L120), [видачу картинок з диска](https://github.com/dotnet/eShop/blob/913acea66708a59595351174cee9b29f37179d29/src/Catalog.API/Apis/CatalogApi.cs#L251-L273) з [визначенням MIME-типу](https://github.com/dotnet/eShop/blob/913acea66708a59595351174cee9b29f37179d29/src/Catalog.API/Apis/CatalogApi.cs#L455-L467), [AI-пошук через embeddings](https://github.com/dotnet/eShop/blob/913acea66708a59595351174cee9b29f37179d29/src/Catalog.API/Apis/CatalogApi.cs#L286-L338) та [оновлення товару з публікацією події про зміну ціни](https://github.com/dotnet/eShop/blob/913acea66708a59595351174cee9b29f37179d29/src/Catalog.API/Apis/CatalogApi.cs#L373-L412).
- Підтримка: клас змінюється з різних причин (HTTP, файли, AI, події), тому зміни різних людей конфліктують, а окремі частини важко тестувати.
- Нова функція: будь-яка нова можливість каталогу знову додається в цей файл, і він росте далі.

**4. Newtonsoft.Json: [JsonSerializerInternalReader](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Serialization/JsonSerializerInternalReader.cs#L53)**
- Де видно: клас на 2769 рядків, крім самої десеріалізації, ще й [розбирає імена типів](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Serialization/JsonSerializerInternalReader.cs#L843), [керує посиланнями $id/$ref](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Serialization/JsonSerializerInternalReader.cs#L1265), [створює об'єкти через конструктори з параметрами](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Serialization/JsonSerializerInternalReader.cs#L2060) та [обробляє помилки](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Serialization/JsonSerializerInternalReader.cs#L2751).
- Підтримка: щоб виправити одну функцію, треба розібратися у великому файлі, а зміна може зламати іншу частину.
- Нова функція: нова можливість десеріалізації майже завжди потребує змін саме в цьому класі.

## Розширення за OCP

**5. eShop: [LoggingBehavior](https://github.com/dotnet/eShop/blob/913acea66708a59595351174cee9b29f37179d29/src/Ordering.API/Application/Behaviors/LoggingBehavior.cs#L2-L15) та [реєстрація поведінок](https://github.com/dotnet/eShop/blob/913acea66708a59595351174cee9b29f37179d29/src/Ordering.API/Extensions/Extensions.cs#L35-L42)**
- Де видно: логування, валідація і транзакції — окремі класи `IPipelineBehavior`, які обгортають кожну команду.
- Підтримка: обробники команд нічого не знають про логування чи транзакції.
- Нова функція: нова наскрізна дія (наприклад, вимірювання часу) — це новий клас і один рядок реєстрації, жоден обробник не змінюється.

**6. Newtonsoft.Json: [JsonConverter](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/JsonConverter.cs#L37-L65) та [вибір конвертера](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/JsonSerializer.cs#L1201-L1220)**
- Де видно: абстрактний клас з методами `ReadJson`, `WriteJson`, `CanConvert`, а серіалізатор просто питає кожен конвертер `CanConvert`.
- Підтримка: бібліотеку не потрібно змінювати для підтримки власних типів.
- Нова функція: користувач пише свій нащадок `JsonConverter` і додає його в `Converters`, код бібліотеки не змінюється.

## Пропозиція рефакторингу

Найгірший приклад — **CatalogApi**: на відміну від `JsonSerializerInternalReader`, який хоч і великий, але весь про десеріалізацію, тут змішано різні шари (HTTP, файлова система, AI, події). Пропоную залишити в `CatalogApi` лише маршрути, а логіку винести в окремі сервіси:

```csharp
public class CatalogPictureService     // картинки з диска та MIME-тип
{
    public Task<(Stream Stream, string MimeType)?> GetPictureAsync(int itemId);
}

public class CatalogSemanticSearch     // AI-пошук через embeddings
{
    public Task<PaginatedItems<CatalogItem>> SearchAsync(string text, PaginationRequest page);
}

public class CatalogItemUpdater        // оновлення товару та подія зміни ціни
{
    public Task<bool> UpdateAsync(int id, CatalogItem newValues);
}

// У CatalogApi метод лише викликає сервіс і повертає HTTP-результат
public static async Task<Results<Created, NotFound>> UpdateItem(int id, CatalogItem item, CatalogItemUpdater updater)
    => await updater.UpdateAsync(id, item) ? TypedResults.Created($"/api/catalog/items/{id}") : TypedResults.NotFound();
```

Тоді кожен клас має одну причину для змін, його можна тестувати окремо, а нова функція каталогу стає новим сервісом замість нових рядків у великому файлі.

---

## Висновок:
В обох репозиторіях SRP найкраще дотримано в маленьких класах з однією дією: обробниках команд eShop і окремих конвертерах Newtonsoft.Json. Порушення SRP з'являються там, де в один клас поступово додавали нові можливості: `CatalogApi` змішує різні шари, а `JsonSerializerInternalReader` збирає всі тонкощі десеріалізації. OCP в обох проєктах реалізовано через абстракцію і список реалізацій, тому нова поведінка додається новим класом без зміни наявного коду. Розмір класу сам по собі не є порушенням SRP: важливіше, зі скількох різних причин клас доводиться змінювати. Найбільше користі від рефакторингу дав би поділ `CatalogApi` на окремі сервіси.
