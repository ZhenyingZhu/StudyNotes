/*
SELECT * FROM AppTestModel;

SELECT AppTestModel.Id, AppTestModel.AppTestInput, AppTestChildModels.Id AS ChildId, AppTestChildModels.Name AS ChildName
FROM AppTestModel LEFT JOIN AppTestChildModels ON AppTestChildModels.AppTestModelId = AppTestModel.Id
WHERE AppTestModel.Id = 10;
*/

SELECT * FROM AppTestChildModels;