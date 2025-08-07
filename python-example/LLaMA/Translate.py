from transformers import NllbTokenizer, AutoModelForSeq2SeqLM

model_name = "facebook/nllb-200-distilled-600M"
tokenizer = NllbTokenizer.from_pretrained(model_name)
model = AutoModelForSeq2SeqLM.from_pretrained(model_name)

text = "日本では、春になると桜が咲きます。"
source_lang = "jpn_Jpan"
target_lang = "zho_Hans"

# Set source language
tokenizer.src_lang = source_lang
inputs = tokenizer(text, return_tensors="pt")

# Set target language via forced_bos_token_id
inputs["forced_bos_token_id"] = tokenizer.lang_code_to_id[target_lang]

# Generate
outputs = model.generate(**inputs)
translated_text = tokenizer.batch_decode(outputs, skip_special_tokens=True)[0]

print(translated_text)