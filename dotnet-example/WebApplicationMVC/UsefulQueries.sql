/*
SELECT * FROM AppTestModel;
SELECT * FROM AppTestChildModels;

SELECT AppTestModel.Id, AppTestModel.AppTestInput, AppTestChildModels.Id AS ChildId, AppTestChildModels.Name AS ChildName
FROM AppTestModel LEFT JOIN AppTestChildModels ON AppTestChildModels.AppTestModelId = AppTestModel.Id
WHERE AppTestModel.Id = 10;
*/
