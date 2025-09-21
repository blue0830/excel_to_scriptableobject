# Ref Excel (Cross-Excel Type Reference) / Ref Excel（跨表引用）

This guide explains how to reference an unknown type in Excel A to a Sheet in Excel B, and the caveats.
本文档介绍如何在主表 A 中，将未知类型字段引用到其他 Excel（B）的同名 Sheet，并给出注意事项。

## What is Ref Excel? / 什么是 Ref Excel？
- EN: Ref Excel lets a field in the main Excel (A) with an unknown type name (e.g. `Buffer` or `Buffer[]`) point to a Sheet with the same name in another Excel (B). The tool records a mapping at generation time and routes lookups at runtime automatically.
- CN: Ref Excel 允许主表 A 中的未知类型（如 `Buffer`/`Buffer[]`）引用到其它 Excel（B）中的同名 Sheet（如 `Buffer`）。生成阶段建立映射，运行时自动路由到外部资源。

## Setup Steps / 配置步骤
1) Add Ref Excels in the editor / 在编辑器中添加 Ref：
- EN: Open the Excel tool window and, in the setting of Excel A, add one or more entries under "Ref Excels". Select the referenced Excel files (e.g. `B.xlsx`). Order matters; first match wins.
- CN: 打开管理窗口，在 A 的设置中添加 “Ref Excels” 条目，选择被引用的 Excel（如 `B.xlsx`）。可添加多个；顺序生效，先命中先用。

2) Author your sheets / 编写表：
- EN: In A.xlsx, define a column with type `Buffer` or `Buffer[]`. In B.xlsx, create a Sheet named `Buffer` whose first column is the key (int/long/string), and fill rows normally.
- CN: 在 A.xlsx 中，字段类型填写 `Buffer` 或 `Buffer[]`；在 B.xlsx 中建立名为 `Buffer` 的 Sheet，第一列为键（int/long/string），正常填充数据。

3) Generate / 生成：
- EN: Click Process Excel for A (and B if first time). The tool writes an internal map Type→Excel for unknown types resolved by refs and generates routing code.
- CN: 对 A（与首次的 B）执行生成。工具会将通过 refs 命中的未知类型记入映射，并生成路由逻辑。

## How to Use at Runtime / 运行时如何使用
- EN: Load both assets as usual. Accessing A.GetBuffer(id) or A.GetBufferList(id) will automatically fetch from B’s asset when the type is mapped externally.
- CN: 正常加载 A 与 B 的资源。调用 `A.GetBuffer(id)` 或 `A.GetBufferList(id)` 时，如该类型映射到外部，将自动从 B 的资源返回数据。

## Behavior and Rules / 行为与规则
- EN:
  - Namespaces: By default A and B share the same namespace for generated types.
  - Multiple refs: You can configure multiple Ref Excels; the first that contains the target Sheet name is used.
  - Key types: The key of referenced Sheet must be int/long/string, same as normal sheets.
  - Fallback: If no ref matches and unknown types are not treated as enum, the build behaves as before and may error for unknown types.
- CN:
  - 命名空间：默认 A 与 B 的生成类型共享同一命名空间。
  - 多个 ref：可配置多个，被引用 Sheet 同名冲突时按配置顺序选择第一个。
  - 键类型：被引用 Sheet 的第一列键类型必须是 int/long/string。
  - 回退：若未命中且未开启“未知类型按枚举处理”，行为与原先一致，未知类型将按原方案报错或处理。

## Examples / 示例
- EN:
  - In A.xlsx: type column is `Buffer[]`, values like `[10010,10011]`.
  - In B.xlsx (Sheet `Buffer`): keys `10010`, `10011` exist; items are returned via `A.GetBufferList(id)`.
- CN:
  - A.xlsx：字段类型为 `Buffer[]`，值如 `[10010,10011]`。
  - B.xlsx（Sheet `Buffer`）：存在键 `10010`、`10011`；运行时通过 `A.GetBufferList(id)` 返回对应条目数组。

## Caveats / 注意事项
- EN:
  - Ensure the referenced Excel (B) is processed and its asset is available at runtime; otherwise routed lookups will fail.
  - If both A and B contain an internal Sheet named `Buffer`, internal data is used unless the type is mapped as external via refs.
  - Changing Sheet names or moving files requires re-processing to refresh mappings.
- CN:
  - 确保被引用的 Excel（B）已生成并在运行时可加载，否则路由查询会失败。
  - 若 A 与 B 同时存在内部 `Buffer`，除非通过 refs 将类型映射为外部，否则默认使用内部数据。
  - 变更 Sheet 名称或移动文件后需重新生成以刷新映射。

## Troubleshooting / 故障排查
- EN:
  - Unknown type error: Add the proper Excel to Ref Excels or enable Treat Unknown Types as Enum if that’s your intent.
  - Data missing at runtime: Confirm B’s asset is loaded/registered and the keys exist.
- CN:
  - 未知类型报错：在 A 的配置中添加正确的 Ref Excel，或按需启用“Treat Unknown Types as Enum”。
  - 运行时缺数据：确认 B 的资源已加载/注册，且键存在。
