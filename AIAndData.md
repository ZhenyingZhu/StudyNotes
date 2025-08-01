# Artificial Intelligence and Big Data

## Machine Learning

<http://open.163.com/special/opencourse/machinelearning.html>

## Big Data

### Map Reduce

Map:

- Key: file location
- Value: file content

Reduce:

- Key, value: OutputCollector returned by Map, which values are already sorted, and only for this key.

```java
void Map::map(Key key, Value value, OutputCollector<ReduceKey, ReduceValue> output);

void Reduce::reduce(ReduceKey key, Iterator<ReduceValue> values, OutputCollector<OutputKey, OutputValue> output);
```

#### Simple example

<http://www.jiuzhang.com/solutions/word-count/>

<http://www.jiuzhang.com/solutions/inverted-index-map-reduce/>

<http://www.jiuzhang.com/solutions/anagram-map-reduce/>

#### BFS

<http://www.johnandcailin.com/blog/cailin/breadth-first-graph-search-using-iterative-map-reduce-algorithm>

```java
void map(int nodeID, string nodeStruct, OutputCollector<int, string> output) {
    Node node = parseStruct(nodeStruct); // edges|dist|status
    if (node.status == visiting) {
        for (int neiID : node.edges) {
            Node nei = {NULL, node.dist + 1, visiting};
            output.collect(neiID, nei.to_string());
        }
        node.status= visited;
    }
    output.collect(nodeID, node.to_string());
}

void reducer(int nodeID, Iterator<string> nodeStructs, OutputCollector<int, string> output) {
    Node node;
    for (string structStr : nodeStructs) {
        Node nodeStruct = parseStruct(structStr);
        if (nodeStruct.edges != NULL)
            node.edges = nodeStruct.edges;
        if (nodeStruct.dist < node.dist) // default is INT_MAX
            node.dist = nodeStruct.dist;
        if (nodeStruct.status > node.status) // visited > visiting > none
            node.status = nodeStruct.status;
    }
    output.collect(nodeID, node.to_string());
}
```

### Spark

<https://timilearning.com/posts/mit-6.824/lecture-15-spark/>

## AI

techniques:

- machine learning
- deep learning
- neural networks
- natural language processing

Generative AI

- Language model: encapsulate semantic relationships between language elements. large language models vs. SLM

Computer vision

- Image classification
- Object detection: OCR
- Image segmentation

Speech

- Speech recognition
- Speech synthesis: text-to-voice

Natural language processing

- not necessarily using GenAI
- entity extraction
- Text classification
- Sentiment analysis

Products:

- Udio: From composing music based on a couple of quick prompts
- Obsidian: taking notes and organizing your thoughts
- Tray.ai: automating and connecting services
- Cursor: dedicated programs
- M365 Copilot: sensitive internal corp information

<https://learningpath.microsoft.com/8523/6de4a674-2336-4ebb-8998-faf7eacb1d23>

## Chat GPT

<https://platform.openai.com/docs/quickstart>

## Github Copilot

<https://github.com/features/copilot>

- /tests Generate unit tests for this function. Validate both success and failure, and include edge cases.
- Improve the variable names in this function. Can use copilot edits

<https://docs.github.com/en/copilot/quickstart?tool=vscode>

- Ask Copilot for help using the command line
- knowledge bases: Create and manage collections of documentation
- Code completion: next edit suggestions
- Copilot Edits: across multiple files directly from a single Copilot Chat prompt
  - Edit mode vs. agent mode

<https://docs.github.com/en/copilot/using-github-copilot/best-practices-for-using-github-copilot>

- Code completions
- built-in keywords
- tell Copilot Chat that it is a Senior C++ Developer who cares greatly about code quality, readability, and efficiency, then ask it to review your code
- Break down complex tasks.
- Be specific about your requirements.
- Provide examples of things like input data, outputs, and implementations.
- Follow good coding practices.
- Review suggestion.
- open relevant files and close irrelevant files.

<https://code.visualstudio.com/blogs/2025/02/24/introducing-copilot-agent-mode>

- `.github/copilot-instructions.md` file to let AI understand about the project

<https://github.com/modelcontextprotocol/servers>

<https://modelcontextprotocol.io/introduction>

- MCP hosts: IDEs, programs
- MCP clients: protocol clients maintaining connections
- MCP servers: services work with MCP like git
- Local resources/internet services

<https://github.com/modelcontextprotocol/python-sdk?tab=readme-ov-file>

<https://docs.astral.sh/uv/>

## Cline

<https://docs.cline.bot/getting-started/for-new-coders>

- `Cline: Open In New Tab`
- `Hello Cline! I need help setting up my Windows PC for software development. Could you please help me install the essential development tools like Node.js, Git, and any other core utilities that are commonly needed for coding? I'd like you to guide me through the process step-by-step.`
- token: 3/4 of a word in English

<https://docs.cline.bot/improving-your-prompting-skills/prompting>

- `.clinerules` file

<https://docs.cline.bot/improving-your-prompting-skills/cline-memory-bank>

Can let it create a MCP server.

- Define core MCP server functionality
- Create basic server structure
- Implement protocol handling
- Add context management

FastAPI is a modern, high-performance web framework for building APIs with Python 3.7+ based on standard Python type hints.

Any AI tools for Outlook? Copilot is very stupid.

Use Microsoft Graph API + Python/OpenAI/GPT to extract and classify emails.

AI for writting doc

AI genetate build target

CLine rules: create a file.

When using CLine (or any LLM-based tool like it) with a multi-step prompt, it may miss steps due to several common reasons:

Token Limitations or Prompt Truncation: If the prompt is too long or complex, parts of it may be ignored or truncated internally. This can lead to steps being skipped, especially near the end of the prompt.

Lack of State Tracking: CLine doesn’t inherently track progress or maintain a checklist of what it has completed. If multiple instructions are given without enforcing sequential execution, it may selectively execute or ignore some.

Ambiguity or Overload: Prompts that include too many actions or vague instructions (e.g., "migrate this project to SDK style") can lead the model to interpret the intent broadly and miss specific tasks. The model might consider some steps implied or unnecessary based on its interpretation.

Execution Environment Side Effects: Sometimes the tool may appear to skip a step because the command silently failed or had no effect in the actual environment. The model might proceed under the assumption that it succeeded.

Heuristic Prioritization: LLMs sometimes prioritize what they believe to be the "core" or "most relevant" actions based on pattern recognition and training. This can cause lower-priority or less common steps to be dropped.

How to Mitigate
Break large prompts into smaller, focused ones, each handling 1–3 steps.

Include explicit checklist-style instructions: “Step 1: … Step 2: … Step 3: …”

Ask CLine to echo or log each step it performs (echo "Starting step 1...") to detect omissions.

Add verification checks at the end (e.g., test builds) to confirm the steps were executed.

## A2A

<https://github.com/google/A2A>

## Translate

- <https://www.youtube.com/watch?v=B3SZCV0IwHU>
- <https://docs.2sj.ai/draw/nono>
- <https://arxiv.org/abs/1706.03762>

## Model difference

Cloude 4 seems the best.

## AI Training

NVIDIA CUDA

- A parallel computing platform for using NVIDIA GPUs.
- Used with deep learning frameworks like PyTorch or TensorFlow.

## Agent Id

<https://techcommunity.microsoft.com/blog/microsoft-entra-blog/announcing-microsoft-entra-agent-id-secure-and-manage-your-ai-agents/3827392>

## Use case

Use LLaMA 3 8B for translate. Hugging Face + bitsandbytes (GPU + CPU hybrid).

```bash
pip install torch transformers accelerate bitsandbytes
pip install huggingface_hub
python -m venv llama3env
.\llama3env\Scripts\activate
pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu128
```

On Windows, bitsandbytes might not work natively.

Need request access to the repo first, then run `huggingface-cli login`

Can check if CUDA is enabled

```python
import torch
print(torch.cuda.is_available())
```

```python
from transformers import AutoTokenizer, AutoModelForCausalLM, pipeline, BitsAndBytesConfig

model_id = "meta-llama/Meta-Llama-3-8B-Instruct"

bnb_config = BitsAndBytesConfig(
    load_in_4bit=True, # quantized loading
    bnb_4bit_quant_type="fp4",  # for GPU
    bnb_4bit_compute_dtype="float16",
    bnb_4bit_use_double_quant=True
)

tokenizer = AutoTokenizer.from_pretrained(model_id, use_auth_token=True)

model = AutoModelForCausalLM.from_pretrained(
    model_id,
    quantization_config=bnb_config,
    device_map="auto",        # offload to available devices
    use_auth_token=True
)

pipe = pipeline("text-generation", model=model, tokenizer=tokenizer)
response = pipe("Translate Japanese to Chinese: 日本では、春になると桜が咲きます。", max_new_tokens=100)
print(response[0]["generated_text"])
```

<https://pytorch.org/get-started/locally/>

`max_new_tokens=1000` can make a big difference.

Difference between deepseek-ai/DeepSeek-R1-Distill-Llama-8B and deepseek-ai/DeepSeek-R1-Distill-Qwen-7B

- Architecture between Meta’s LLaMA 3 or Alibaba’s Qwen (stronger in Chinese)

```python
messages = [
    {"role": "user", "content": "请把这句日文翻译成中文:\n日本では、春になると桜が咲きます。"}
]

input_ids = tokenizer.apply_chat_template(messages, return_tensors="pt").to("cuda")
output_ids = model.generate(input_ids, max_new_tokens=5000)
response = tokenizer.decode(output_ids[0], skip_special_tokens=True)
```

Cache folder: `C:\Users\<user>\.cache\huggingface\hub\`

Got error: `ValueError: Unrecognized configuration class <class 'transformers.models.m2m_100.configuration_m2m_100.M2M100Config'> for this kind of AutoModel: AutoModelForCausalLM.`

To use it, `pip install sentencepiece`, `python -m pip install --upgrade pip setuptools wheel`

## Concept

A model is a large mathematical function trained to understand and generate data.

- LLaMA and GPT are 2 models
- A giant neural network with billions of parameters, that learns probabilities of sequences of words/tokens
- training data saved in .bin, .safetensors, or .gguf
- The blueprint: how many layers, attention heads, embedding size, etc.
