//by Robert C. Martin

public class ResponseParser {
  private int status;
  private String body;
  private HashMap<String, String> headers = new HashMap<String, String>();
  private StreamReader input;

  private static final Pattern statusLinePattern = Pattern.compile("HTTP/\\d.\\d (\\d\\d\\d) ");
  private static final Pattern headerPattern = Pattern.compile("([^:]*): (.*)");

  public ResponseParser(InputStream input) throws Exception {
    this.input = new StreamReader(input);
    parseStatusLine();
    parseHeaders();
    if (isChuncked()) {
      parseChunks();
      parseHeaders();
    } else
      parseBody();
  }
  
  public int getStatus() {
    return status;
  }

  public String getBody() {
    return body;
  }

  public String getHeader(String key) {
    return (String) headers.get(key);
  }

  public boolean hasHeader(String key) {
    return headers.containsKey(key);
  }

  private boolean isChuncked() {
    String encoding = getHeader("Transfer-Encoding");
    return encoding != null && "chunked".equals(encoding.toLowerCase());
  }

  private void parseStatusLine() throws Exception {
    String statusLine = input.readLine();
    Matcher match = statusLinePattern.matcher(statusLine);
    if (match.find()) {
      String status = match.group(1);
      this.status = Integer.parseInt(status);
    } else
      throw new Exception("Could not parse Response");
  }

  private void parseHeaders() throws Exception {
    String line = input.readLine();
    while (!"".equals(line)) {
      Matcher match = headerPattern.matcher(line);
      if (match.find()) {
        String key = match.group(1);
        String value = match.group(2);
        headers.put(key, value);
      }
      line = input.readLine();
    }
  }
  
  //refactoring example 1
  private void parseHeaders() throws Exception {
    String line = input.readLine();
	while (!"".equals(line)) {
	  Header header = ResponseLineParser.getHeaderFrom(line);
      if (header != null){
		headers.put(header.key, header.value);
	  }
      line = input.readLine();
    }
  }
  
  //refactoring example 2
  private void parseHeaders() throws Exception {
    List<Headers> headers= ResponseLineParser.getHeadersFrom(getHeaderLines());
    Iterator it = headers.iterator();
    while (it.hasNext()) {
      Header header = it.next();
      headers.put(header.key, header.value);
    }
  }

  private void parseBody() throws Exception {
    String lengthHeader = "Content-Length";
    if (hasHeader(lengthHeader)) {
      int bytesToRead = Integer.parseInt(getHeader(lengthHeader));
      body = input.read(bytesToRead);
    }
  }

  private void parseChunks() throws Exception {
    StringBuffer bodyBuffer = new StringBuffer();
    int chunkSize = readChunkSize();
    while (chunkSize != 0) {
      bodyBuffer.append(input.read(chunkSize));
      readCRLF();
      chunkSize = readChunkSize();
    }
    body = bodyBuffer.toString();

  }

  private int readChunkSize() throws Exception {
    String sizeLine = input.readLine();
    return Integer.parseInt(sizeLine, 16);
  }

  private void readCRLF() throws Exception {
    input.read(2);
  }
 }